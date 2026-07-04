## gstack (REQUIRED — global install)

**Before doing ANY work, verify gstack is installed:**

```bash
test -d ~/.claude/skills/gstack/bin && echo "GSTACK_OK" || echo "GSTACK_MISSING"
```

If GSTACK_MISSING: STOP. Do not proceed. Tell the user:

> gstack is required for all AI-assisted work in this repo.
> Install it:
> ```bash
> git clone --depth 1 https://github.com/garrytan/gstack.git ~/.claude/skills/gstack
> cd ~/.claude/skills/gstack && ./setup --team
> ```
> Then restart your AI coding tool.

Do not skip skills, ignore gstack errors, or work around missing gstack.

Using gstack skills: After install, skills like /qa, /ship, /review, /investigate,
and /browse are available. Use /browse for all web browsing.
Use ~/.claude/skills/gstack/... for gstack file paths (the global path).

## Autonomous agent rules

When working autonomously:

1. Use gstack.
2. Run /autoplan before making code changes.
3. Make a new branch for every task.
4. Branch names should use this format: agent/<short-task-name>.
5. Keep changes small and focused.
6. Prefer modifying existing patterns over inventing new architecture.
7. Add or update tests when behavior changes.
8. Run the project’s relevant tests before opening a PR.
9. Run /review before shipping.
10. Run /ship only after tests pass.
11. Open PRs as drafts by default.
12. Never merge PRs automatically.
13. Never commit secrets, API keys, tokens, .env files, or local credentials.
14. Never edit production deployment configuration unless the issue explicitly asks for it.
15. If tests fail, include the failure in the PR body instead of hiding it.
16. If the task is ambiguous, make the smallest reasonable interpretation and document it in the PR.
17. Do not rewrite unrelated code.
18. Do not upgrade major dependencies unless specifically asked.

## gstack

Use /browse from gstack for all web browsing.
Available gstack skills include /office-hours, /autoplan, /review, /qa, /ship, /investigate, /careful, /freeze, /guard, /unfreeze, and /gstack-upgrade.

## PR requirements

Every PR must include:

- Summary
- What changed
- Tests run
- Any known issues
- Screenshots or output if relevant
