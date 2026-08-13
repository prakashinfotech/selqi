import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TitleCasePipe } from '@angular/common';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BasketService } from '../../core/services/basket.service';
import { OrderService } from '../../core/services/order.service';
import { AddressService } from '../../core/services/address.service';
import { SavedCardService } from '../../core/services/saved-card.service';
import { RazorpayService } from '../../core/services/razorpay.service';
import { DeliveryAddress } from '../../shared/models/order.model';
import { SavedAddress } from '../../shared/models/auth.model';
import { SavedCard } from '../../shared/models/saved-card.model';

type PaymentMethod = 'card' | 'razorpay';

type Step = 'address' | 'delivery' | 'payment' | 'review';
type FieldErrors = Partial<Record<string, string>>;

const POSTCODE_RE = /^[A-Za-z]{1,2}\d[\dA-Za-z]?\s*\d[A-Za-z]{2}$/;
const PHONE_RE = /^[\+\d][\d\s\-\(\)]{6,18}$/;
const CARD_RE = /^\d{13,19}$/;
const MONTH_RE = /^(0[1-9]|1[0-2])$/;
const YEAR_RE = /^\d{2}$/;
const CVV_RE = /^\d{3,4}$/;

function detectBrand(digits: string): string {
  if (/^4/.test(digits)) return 'Visa';
  if (/^5[1-5]/.test(digits) || /^2[2-7]/.test(digits)) return 'Mastercard';
  if (/^3[47]/.test(digits)) return 'Amex';
  if (/^6(?:011|5)/.test(digits)) return 'Discover';
  return 'Card';
}

@Component({
  selector: 'app-checkout',
  imports: [FormsModule, TitleCasePipe],
  templateUrl: './checkout.html',
  styleUrl: './checkout.scss'
})
export class Checkout implements OnInit {
  private readonly router = inject(Router);
  private readonly basketSvc = inject(BasketService);
  private readonly orderSvc = inject(OrderService);
  private readonly addressSvc = inject(AddressService);
  private readonly cardSvc = inject(SavedCardService);
  private readonly razorpaySvc = inject(RazorpayService);

  protected readonly basket = this.basketSvc.basket;
  protected readonly step = signal<Step>('address');
  protected readonly submitting = signal(false);
  protected readonly serverError = signal('');

  // Saved address state
  protected savedAddresses = signal<SavedAddress[]>([]);
  protected savedAddressesLoaded = signal(false);
  protected selectedSavedAddressId = signal<string | null>(null);
  protected showNewAddressForm = signal(false);
  protected saveNewAddress = signal(false);
  protected newAddressLabel = '';

  // Payment method selection
  protected paymentMethod = signal<PaymentMethod>('razorpay');

  // Saved card state
  protected savedCards = signal<SavedCard[]>([]);
  protected savedCardsLoaded = signal(false);
  protected selectedCardId = signal<string | null>(null);
  protected showNewCardForm = signal(false);
  protected saveNewCard = signal(false);

  // Form data
  protected address: DeliveryAddress = {
    firstName: '', lastName: '', line1: '', line2: '',
    town: '', county: '', postcode: '', phone: ''
  };
  protected deliveryOption: 'standard' | 'express' = 'standard';
  protected payment = { cardNumber: '', cardHolder: '', expiryMonth: '', expiryYear: '', cvv: '' };

  // Per-field validation errors
  protected addressErrors = signal<FieldErrors>({});
  protected paymentErrors = signal<FieldErrors>({});

  protected readonly deliveryCharge = computed(() => {
    if (this.deliveryOption === 'express') return 5.95;
    return (this.basket()?.subTotal ?? 0) >= 50 ? 0 : 3.95;
  });

  protected readonly orderTotal = computed(() =>
    (this.basket()?.subTotal ?? 0) + this.deliveryCharge()
  );

  protected readonly steps: Step[] = ['address', 'delivery', 'payment', 'review'];
  protected readonly stepIndex = computed(() => this.steps.indexOf(this.step()));

  protected readonly reviewPaymentLabel = computed(() => {
    if (this.paymentMethod() === 'razorpay') return 'Pay via Razorpay (test mode)';
    const card = this.selectedCardId()
      ? this.savedCards().find(c => c.id === this.selectedCardId())
      : null;
    if (card) return `${card.brand} ending ···· ${card.last4} — ${card.cardholderName}`;
    const digits = this.payment.cardNumber.replace(/\s/g, '');
    const last4 = digits.slice(-4);
    if (!last4) return 'No card entered';
    return `${detectBrand(digits)} ending ···· ${last4} — ${this.payment.cardHolder}`;
  });

  ngOnInit(): void {
    const b = this.basket();
    if (!b) {
      this.basketSvc.load().subscribe({
        next: basket => { if (!basket || basket.items.length === 0) this.router.navigate(['/basket']); }
      });
    } else if (b.items.length === 0) {
      this.router.navigate(['/basket']);
    }

    forkJoin({
      addresses: this.addressSvc.getAddresses().pipe(catchError(() => of([] as SavedAddress[]))),
      cards: this.cardSvc.getCards().pipe(catchError(() => of([] as SavedCard[])))
    }).subscribe(({ addresses, cards }) => {
      this.savedAddresses.set(addresses);
      this.savedAddressesLoaded.set(true);
      const def = addresses.find(a => a.isDefault) ?? addresses[0];
      if (def) this.selectSavedAddress(def.id);
      else this.showNewAddressForm.set(true);

      this.savedCards.set(cards);
      this.savedCardsLoaded.set(true);
      if (cards.length > 0) {
        const defaultCard = cards.find(c => c.isDefault) ?? cards[0];
        this.selectSavedCard(defaultCard.id);
      } else {
        this.showNewCardForm.set(true);
      }
    });
  }

  // ── Address helpers ──

  protected selectSavedAddress(id: string): void {
    const saved = this.savedAddresses().find(a => a.id === id);
    if (!saved) return;
    this.selectedSavedAddressId.set(id);
    this.showNewAddressForm.set(false);
    this.address = {
      firstName: saved.firstName,
      lastName: saved.lastName,
      line1: saved.line1,
      line2: saved.line2 ?? '',
      town: saved.town,
      county: saved.county ?? '',
      postcode: saved.postcode,
      phone: saved.phone
    };
    this.addressErrors.set({});
  }

  protected useNewAddress(): void {
    this.selectedSavedAddressId.set(null);
    this.showNewAddressForm.set(true);
    this.address = { firstName: '', lastName: '', line1: '', line2: '', town: '', county: '', postcode: '', phone: '' };
    this.addressErrors.set({});
  }

  protected clearAddressError(field: string): void {
    const cur = this.addressErrors();
    if (cur[field]) { const u = { ...cur }; delete u[field]; this.addressErrors.set(u); }
  }

  // ── Payment method toggle ──

  protected selectPaymentMethod(method: PaymentMethod): void {
    this.paymentMethod.set(method);
    this.paymentErrors.set({});
  }

  // ── Card helpers ──

  protected selectSavedCard(id: string): void {
    this.selectedCardId.set(id);
    this.showNewCardForm.set(false);
    this.paymentErrors.set({});
  }

  protected useNewCard(): void {
    this.selectedCardId.set(null);
    this.showNewCardForm.set(true);
    this.payment = { cardNumber: '', cardHolder: '', expiryMonth: '', expiryYear: '', cvv: '' };
    this.paymentErrors.set({});
  }

  protected clearPaymentError(field: string): void {
    const cur = this.paymentErrors();
    if (cur[field]) { const u = { ...cur }; delete u[field]; this.paymentErrors.set(u); }
  }

  // ── Navigation ──

  protected nextStep(): void {
    const idx = this.stepIndex();
    if (idx === 0 && !this.validateAddress()) return;
    if (idx === 2 && !this.validatePayment()) return;

    if (idx === 0 && this.showNewAddressForm() && this.saveNewAddress()) {
      this.persistNewAddress();
    }

    if (idx < this.steps.length - 1) {
      this.step.set(this.steps[idx + 1]);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  protected goTo(s: Step): void {
    const target = this.steps.indexOf(s);
    if (target <= this.stepIndex()) this.step.set(s);
  }

  // ── Place order ──

  protected placeOrder(): void {
    if (this.paymentMethod() === 'razorpay') {
      this.placeOrderViaRazorpay();
    } else {
      this.placeOrderViaCard();
    }
  }

  private placeOrderViaRazorpay(): void {
    this.submitting.set(true);
    this.serverError.set('');

    this.razorpaySvc.createOrder(this.deliveryOption).subscribe({
      next: rzpOrder => {
        this.submitting.set(false);
        this.razorpaySvc.openCheckout(
          rzpOrder,
          `${this.address.firstName} ${this.address.lastName}`,
          this.address.phone,
          (response) => {
            this.submitting.set(true);
            this.razorpaySvc.verifyPayment({
              razorpayOrderId: response.razorpay_order_id,
              razorpayPaymentId: response.razorpay_payment_id,
              razorpaySignature: response.razorpay_signature,
              deliveryAddress: this.address,
              deliveryOption: this.deliveryOption
            }).subscribe({
              next: order => {
                this.basketSvc.reset();
                this.router.navigate(['/orders', order.orderNumber]);
              },
              error: (err) => {
                this.serverError.set(err?.error?.message ?? 'Payment verified but order creation failed. Please contact support.');
                this.submitting.set(false);
              }
            });
          },
          () => {
            this.serverError.set('Payment was cancelled. You have not been charged.');
            this.submitting.set(false);
          }
        );
      },
      error: (err) => {
        this.serverError.set(err?.error?.message ?? 'Could not initiate payment. Please try again.');
        this.submitting.set(false);
      }
    });
  }

  private placeOrderViaCard(): void {
    this.submitting.set(true);
    this.serverError.set('');

    const cardId = this.selectedCardId();
    const digits = this.payment.cardNumber.replace(/\s/g, '');

    this.orderSvc.placeOrder({
      deliveryAddress: this.address,
      deliveryOption: this.deliveryOption,
      payment: cardId
        ? { savedCardId: cardId }
        : {
            cardNumber: digits.slice(-4),
            cardHolder: this.payment.cardHolder,
            expiryMonth: this.payment.expiryMonth,
            expiryYear: this.payment.expiryYear,
            saveCard: this.saveNewCard(),
            cardBrand: detectBrand(digits)
          }
    }).subscribe({
      next: order => {
        this.basketSvc.reset();
        this.router.navigate(['/orders', order.orderNumber]);
      },
      error: (err) => {
        this.serverError.set(err?.error?.message ?? 'Failed to place order. Please try again.');
        this.submitting.set(false);
      }
    });
  }

  // ── Validation ──

  private validateAddress(): boolean {
    const errs: FieldErrors = {};

    if (!this.address.firstName.trim()) errs['firstName'] = 'First name is required.';
    else if (this.address.firstName.trim().length < 2) errs['firstName'] = 'Must be at least 2 characters.';

    if (!this.address.lastName.trim()) errs['lastName'] = 'Last name is required.';
    else if (this.address.lastName.trim().length < 2) errs['lastName'] = 'Must be at least 2 characters.';

    if (!this.address.line1.trim()) errs['line1'] = 'Address line 1 is required.';
    else if (this.address.line1.trim().length < 5) errs['line1'] = 'Enter a full address (at least 5 characters).';

    if (!this.address.town.trim()) errs['town'] = 'Town or city is required.';

    if (!this.address.postcode.trim()) errs['postcode'] = 'Postcode is required.';
    else if (!POSTCODE_RE.test(this.address.postcode.trim())) errs['postcode'] = 'Enter a valid UK postcode (e.g. SW1A 1AA).';

    if (!this.address.phone.trim()) errs['phone'] = 'Phone number is required.';
    else if (!PHONE_RE.test(this.address.phone.trim())) errs['phone'] = 'Enter a valid phone number.';

    this.addressErrors.set(errs);
    return Object.keys(errs).length === 0;
  }

  private validatePayment(): boolean {
    // Razorpay handles its own card entry — nothing to validate here
    if (this.paymentMethod() === 'razorpay') return true;
    // Saved card selected — no card entry needed
    if (this.selectedCardId()) return true;

    const errs: FieldErrors = {};
    const cardDigits = this.payment.cardNumber.replace(/\s/g, '');
    const currentYear = new Date().getFullYear() % 100;

    if (!this.payment.cardNumber.trim()) errs['cardNumber'] = 'Card number is required.';
    else if (!CARD_RE.test(cardDigits)) errs['cardNumber'] = 'Enter a valid card number (13–19 digits).';

    if (!this.payment.cardHolder.trim()) errs['cardHolder'] = 'Cardholder name is required.';
    else if (this.payment.cardHolder.trim().length < 3) errs['cardHolder'] = 'Name must be at least 3 characters.';

    if (!this.payment.expiryMonth) errs['expiryMonth'] = 'Expiry month is required.';
    else if (!MONTH_RE.test(this.payment.expiryMonth)) errs['expiryMonth'] = 'Enter a valid month (01–12).';

    if (!this.payment.expiryYear) errs['expiryYear'] = 'Expiry year is required.';
    else if (!YEAR_RE.test(this.payment.expiryYear)) errs['expiryYear'] = 'Enter last 2 digits (e.g. 27).';
    else if (parseInt(this.payment.expiryYear, 10) < currentYear) errs['expiryYear'] = 'This card has expired.';

    if (!this.payment.cvv) errs['cvv'] = 'Security code is required.';
    else if (!CVV_RE.test(this.payment.cvv)) errs['cvv'] = 'Enter the 3 or 4 digit security code.';

    this.paymentErrors.set(errs);
    return Object.keys(errs).length === 0;
  }

  private persistNewAddress(): void {
    const label = this.newAddressLabel.trim() || 'Home';
    this.addressSvc.saveAddress({
      label,
      firstName: this.address.firstName.trim(),
      lastName: this.address.lastName.trim(),
      line1: this.address.line1.trim(),
      line2: this.address.line2?.trim() || null,
      town: this.address.town.trim(),
      county: this.address.county?.trim() || null,
      postcode: this.address.postcode.trim().toUpperCase(),
      country: 'United Kingdom',
      phone: this.address.phone.trim(),
      isDefault: this.savedAddresses().length === 0
    }).subscribe({
      next: saved => this.savedAddresses.update(list => [...list, saved]),
      error: () => { /* silently ignore — non-critical */ }
    });
  }
}
