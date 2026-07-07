# Web (planning scaffold)

Reserved space for a future Nutrino web app. No framework has been chosen yet
and no application code lives here — this is folder scaffolding only, so
eventual web deployment work has a place to land without restructuring the
repo.

- `src/` — application source (once a web framework is chosen, e.g. Next.js,
  Remix, SvelteKit, or a plain SPA)
- `public/` — static assets served as-is (images, favicon, manifest, etc.)
- `deploy/` — deployment configuration for this app (Dockerfile, platform
  config such as `vercel.json`/`fly.toml`/`Procfile`, CI deploy workflow
  fragments, env var templates)
- `docs/` — web-specific architecture notes, decisions, and specs

Framework and hosting choice are separate decisions — see `/spec` or
`/plan-eng-review` before starting real implementation or deployment
configuration here.
