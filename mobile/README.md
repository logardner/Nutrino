# Mobile (planning scaffold)

Reserved space for a future Nutrino mobile app. No framework has been chosen yet
and no application code lives here — this is folder scaffolding only, so the
eventual mobile work has a place to land without restructuring the repo.

- `ios/` — native iOS project (if a native or split-native approach is chosen)
- `android/` — native Android project (if a native or split-native approach is chosen)
- `shared/` — cross-platform code (e.g. a React Native or Flutter app, or shared
  API clients/types/design tokens used by both platforms)
- `docs/` — mobile-specific architecture notes, decisions, and specs

Framework choice (React Native, Flutter, native, etc.) is a separate decision —
see `/spec` or `/plan-eng-review` before starting real implementation here.
