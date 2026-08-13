import { Component, inject, OnInit, OnDestroy, AfterViewInit, signal, ViewChild, ElementRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CatalogService } from '../../core/services/catalog.service';
import { RecommendationService } from '../../core/services/recommendation.service';
import { ProductSummary } from '../../shared/models/product.model';
import { AiSuggestion } from '../../shared/models/recommendation.model';
import { ProductQuery } from '../../shared/models/product.model';
import { PaginationConstants } from '../../shared/constants/pagination.constants';
import { CatalogConstants } from '../../shared/constants/catalog.constants';

interface HeroSlide {
  id: number;
  theme: string;
  chip: string | null;
  heading: string;
  body: string;
  ctaLabel: string;
  ctaLink: string;
  imageUrl: string;
  imageAlt: string;
}

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit, AfterViewInit, OnDestroy {
  protected readonly catalog = inject(CatalogService);
  private readonly recommendations = inject(RecommendationService);

  protected readonly popularProducts = signal<ProductSummary[]>([]);
  protected readonly productsLoaded = signal(false);
  protected readonly aiSuggestions = signal<AiSuggestion[]>([]);
  protected readonly suggestionsLoaded = signal(false);
  protected readonly suggestionsError = signal(false);
  protected readonly currentSlide = signal(0);
  protected readonly isPaused = signal(false);
  protected readonly canScrollLeft = signal(false);
  protected readonly canScrollRight = signal(true);

  @ViewChild('quickScroll') private quickScrollRef!: ElementRef<HTMLElement>;

  private timer: ReturnType<typeof setInterval> | null = null;

  protected readonly slides: HeroSlide[] = [
    {
      id: 1, theme: 'tech', chip: 'save',
      heading: 'Big value across everyday tech',
      body: 'Browse laptops, tablets, wearables and home devices built from the demo catalog.',
      ctaLabel: 'Shop technology', ctaLink: '/category/technology',
      imageUrl: 'https://images.unsplash.com/photo-1498049794561-7780e7231661?auto=format&fit=crop&w=580&h=360',
      imageAlt: 'Technology products on a desk'
    },
    {
      id: 2, theme: 'home', chip: null,
      heading: 'Refresh your rooms for less',
      body: 'Selected demo furniture, storage and lighting picks to transform your living space.',
      ctaLabel: 'Shop home', ctaLink: '/category/home',
      imageUrl: 'https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=580&h=360',
      imageAlt: 'Modern living room furniture'
    },
    {
      id: 3, theme: 'garden', chip: null,
      heading: 'Get set for your garden',
      body: 'Garden seating, tools and seasonal essentials from the demo outdoor range.',
      ctaLabel: 'Shop garden', ctaLink: '/category/garden',
      imageUrl: 'https://images.unsplash.com/photo-1416879595882-3373a0480b5b?auto=format&fit=crop&w=580&h=360',
      imageAlt: 'A garden in bloom'
    }
  ];

  ngAfterViewInit(): void {
    this.syncScrollState();
  }

  ngOnInit(): void {
    this.catalog.loadCategories();

    this.catalog.getProducts({ pageSize: PaginationConstants.homePopularProductsCount, sort: 'rating' as ProductQuery['sort'] }).subscribe({
      next: result => {
        this.popularProducts.set(result.items);
        this.productsLoaded.set(true);
      },
      error: () => this.productsLoaded.set(true)
    });

    this.recommendations.getSuggestions().subscribe({
      next: suggestions => {
        this.aiSuggestions.set(suggestions);
        this.suggestionsLoaded.set(true);
      },
      error: () => {
        this.suggestionsError.set(true);
        this.suggestionsLoaded.set(true);
      }
    });

    this.startTimer();
  }

  ngOnDestroy(): void {
    this.stopTimer();
  }

  protected goTo(idx: number): void {
    this.currentSlide.set(idx);
    if (!this.isPaused()) this.restartTimer();
  }

  protected prev(): void {
    this.currentSlide.update(i => (i - 1 + this.slides.length) % this.slides.length);
    if (!this.isPaused()) this.restartTimer();
  }

  protected next(): void {
    this.currentSlide.update(i => (i + 1) % this.slides.length);
    if (!this.isPaused()) this.restartTimer();
  }

  protected togglePause(): void {
    if (this.isPaused()) {
      this.isPaused.set(false);
      this.startTimer();
    } else {
      this.isPaused.set(true);
      this.stopTimer();
    }
  }

  private startTimer(): void {
    this.timer = setInterval(() => {
      this.currentSlide.update(i => (i + 1) % this.slides.length);
    }, CatalogConstants.carouselIntervalMs);
  }

  private stopTimer(): void {
    if (this.timer !== null) {
      clearInterval(this.timer);
      this.timer = null;
    }
  }

  private restartTimer(): void {
    this.stopTimer();
    this.startTimer();
  }

  protected scrollQuick(px: number): void {
    this.quickScrollRef?.nativeElement.scrollBy({ left: px, behavior: 'smooth' });
  }

  protected onQuickScroll(): void {
    this.syncScrollState();
  }

  private syncScrollState(): void {
    const el = this.quickScrollRef?.nativeElement;
    if (!el) return;
    this.canScrollLeft.set(el.scrollLeft > CatalogConstants.scrollThresholdPx);
    this.canScrollRight.set(el.scrollLeft + el.clientWidth < el.scrollWidth - CatalogConstants.scrollThresholdPx);
  }

  protected discount(product: ProductSummary): number | null {
    if (!product.wasPrice || product.wasPrice <= product.price) return null;
    return Math.round((1 - product.price / product.wasPrice) * 100);
  }
}
