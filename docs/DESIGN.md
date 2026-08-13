# Design System — Selqi Replica

Reference site: https://www.selqi.co.uk  
Purpose: Learning project — faithful visual replica of the Selqi homepage and header.

---

## Header Structure

The header is a sticky three-tier unit (`<header class="site-header">`).

### Tier 1 — Top utility bar
- Background: `#ffffff`
- Border-bottom: `1px solid #e8e8e8`
- Padding: `5px 0`
- Content: right-aligned links — **Track Order | Returns | Stores | Help**
- Font: 12px, color `#4a4a4a`

### Tier 2 — Main header row
- Background: `#ffffff`
- Border-bottom: `1px solid #e8e8e8`
- Padding: `10px 0`, min-height `52px`
- Layout (left → right, flex, `align-items: center`):
  1. **Logo** — red SVG rectangle (82 × 38 px), white "Selqi" text
  2. **Primary nav** — `Shop ▼` `Trending ▼` (bold buttons, `font-size: 14px`) + `Summer of football` promo link (blue, `#0c509f`)
  3. **Search** — `flex: 1`, fixed height `44px`, border `1px solid #c5c5c5`, left-radius `4px`
     - Input: full-width, `font-size: 14px`, placeholder color `#767676`
     - Button: background `#1c1c1c`, white text, `font-weight: 700`, `padding: 0 24px`
     - Accessibility: `aria-describedby="site-search-hint"` → hidden span with: *"Use up and down arrows to review and enter to select. Touch device users, explore by touch or with swipe gestures."*
  4. **Ask Trevor** — green pill `#006e3c`, rounded `999px`, avatar circle + "Ask Trevor" text
  5. **Actions** (stacked icon + label): Account | Wishlist | Trolley (with badge)

### Tier 3 — Benefits bar
- Background: `#f5f5f5`
- Border-bottom: `1px solid #e8e8e8`
- Padding: `9px 0`
- Layout: centered flex row, items separated by `1px` vertical dividers (`height: 18px`, color `#ccc`)
- Three items (`font-size: 13px`, `font-weight: 500`, color `#333`):
  1. 💳 **Selqi Pay Credit Available**
  2. 📡 **Shop our latest offers**
  3. 🚚 **A year of fast delivery with Selqi Plus**

---

## Home Page Structure

Sections in render order:

### 1. Quick-strip (category tiles)
- Background: `#ffffff`, `border-bottom: 1px solid #e8e8e8`
- Horizontally scrollable row; tiles are `88px` wide, `flex: 0 0 88px`
- **Icon container: `72 × 72 px`, `border-radius: 50%`** (full circle — matches Selqi)
- Default tile background: `var(--color-gray-100)` (`#f3f3f3`)
- Special tiles:
  - `--pay` / `--deals`: red background `var(--color-primary)`, white icon
  - `--sun` (Top Rated): yellow `#fff4c7`, dark text
- Label: `11px`, `font-weight: 650`, centered below icon

### 2. Hero carousel
- Outer background: `#f2f2f2`
- Grid: `2.2fr` (hero panel) + `0.75fr` (side promos), `gap: 12px`
- **Hero panel**: `min-height: 380px`, `border-radius: 8px`
  - Background: `linear-gradient(130deg, #4f46e5 0%, #312e81 100%)` + subtle radial highlight
  - Padding: `32px 40px`
  - Copy area: headline, sub-text, white CTA button
  - Media area: product photo with large drop shadow
- **Side promos**: two stacked cards (`min-height: 168px` each), image + gradient overlay, text at bottom

### 3. Carousel controls
- Background: `#f2f2f2`
- Centered flex row, `gap: 8px`
- Prev/next: `32 × 32 px` circle buttons, `border: 1px solid #ccc`, white fill
- Inactive dot: `8 × 8 px`, `border-radius: 50%`, background `#bbb`
- Active dot: `24 × 8 px` pill, background `#1c1c1c`
- Pause button: text pill, `font-size: 12px`, `font-weight: 600`

### 4. Credit / Selqi Pay strip
- Background: `var(--color-primary)` (#4f46e5)
- Three-column flex: **Selqi Pay** logo chip | body text | CTA link

### 5. Trending now
- 4-column grid, each card `min-height: 250px`
- Cards have full-bleed background images with `linear-gradient` overlay
- Text at bottom: white headline + underlined "Shop X" link

### 6. Popular products
- 6-column grid (`repeat(6, minmax(150px, 1fr))`)
- White bordered cards with lazy-loaded product images
- Skeleton loading animation (`shimmer` keyframe) while API responds

### 7. Spring deals
- 4-column grid, cards `min-height: 220px`, same image-overlay pattern

### 8–11. Brands → Service cards → Pay feature → Store finder → Editorial
- See `home.html` / `home.scss` for exact markup.

---

## Colour Palette

| Token | Value | Usage |
|---|---|---|
| `--color-primary` | `#4f46e5` | Brand indigo — buttons, CTAs, active states |
| `--color-primary-dark` | `#3730a3` | Hover on primary |
| `--color-gray-900` | `#1c1c1c` | Body text, search button |
| `--color-gray-700` | `#4a4a4a` | Secondary text, utility links |
| `--color-gray-500` | `#767676` | Placeholders, muted labels |
| `--color-gray-200` | `#e5e5e5` | Dividers, card borders |
| `--color-gray-100` | `#f3f3f3` | Tile backgrounds, input fills |
| `--color-info` | `#0c509f` | Promo nav links |
| Ask Trevor green | `#006e3c` | Ask Trevor button only |

---

## Typography

- Font stack: `-apple-system, BlinkMacSystemFont, 'Segoe UI', Arial, Helvetica, sans-serif`
- Base size: `16px` / line-height `1.5`
- Weights used: 500 (body), 600–700 (labels, nav), 800–900 (headlines, prices)

---

## Responsive Breakpoints

| Breakpoint | Behaviour |
|---|---|
| `≤ 1100px` | Hides promo nav link; Ask Trevor collapses to icon only |
| `≤ 900px` | Hides primary nav; hides Ask Trevor entirely |
| `≤ 1120px` | Hero and pay-feature grids collapse to 1 column; trend/deal grids → 2 col |
| `≤ 720px` | All grids → 1 column; header actions hide labels |
| `≤ 640px` | Search moves to second row; top bar and benefits bar hidden |

---

## Implementation Notes

- No CSS framework — all styles are custom SCSS with CSS custom properties.
- Components use Angular 21 standalone component pattern.
- Images are Unsplash placeholders; real Selqi assets are proprietary and not used.
- Carousel controls are static (visual-only); functional slide rotation can be added in Phase 2.
