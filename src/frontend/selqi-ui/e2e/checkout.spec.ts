import { expect, test } from '@playwright/test';

test.describe('checkout flow', () => {
  test('allows a customer to browse, add to basket, and reach checkout', async ({ page, request }) => {
    const apiBase = process.env['E2E_API_BASE_URL'] ?? 'http://localhost:5123/api/v1';
    const email = `playwright-${Date.now()}-${Math.random().toString(16).slice(2)}@example.test`;
    const registerResponse = await request.post(`${apiBase}/auth/register`, {
      data: {
        email,
        password: 'Playwright-Local-123!',
        firstName: 'Test',
        lastName: 'Customer'
      }
    });
    expect(registerResponse.ok()).toBeTruthy();
    const auth = await registerResponse.json();

    await page.addInitScript(({ token, user }) => {
      localStorage.setItem('selqi_token', token);
      localStorage.setItem('selqi_user', JSON.stringify(user));
    }, auth);

    await page.goto('/products');
    await expect(page).toHaveURL(/products/);

    await page.locator('a.product-card').first().click();
    await expect(page).toHaveURL(/\/product\//);
    await expect(page.getByRole('button', { name: /add to trolley/i })).toBeVisible();

    await page.getByRole('button', { name: /add to trolley/i }).click();
    await expect(page.getByRole('button', { name: /added to trolley/i })).toBeVisible();
    await page.getByRole('link', { name: /trolley/i }).click();

    await expect(page.getByRole('heading', { name: /trolley|basket/i })).toBeVisible();
    await page.getByRole('button', { name: /checkout securely/i }).click();

    await expect(page).toHaveURL(/checkout/);
  });
});
