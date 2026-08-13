# Agent Guidelines

## Repository Overview

**AuroraScienceHub.Geopack** — a high-performance C# port of the Fortran Geopack-2008dp geomagnetic field library by N. A. Tsyganenko. Produces three NuGet packages:

- `AuroraScienceHub.Geopack.Contracts`
- `AuroraScienceHub.Geopack`
- `AuroraScienceHub.Geopack.ExternalFieldModels`

This is a **library repository** — no runnable app. License: GPL-3.0-or-later.

**Language:** English is the official language of the repository — write commit messages, PR descriptions, documentation, and code comments in English.

---

## Project Structure

```
src/
├── Contracts/              # AuroraScienceHub.Geopack.Contracts
├── Geopack/                # AuroraScienceHub.Geopack (depends on Contracts)
├── ExternalFieldModels/    # AuroraScienceHub.Geopack.ExternalFieldModels (depends on Contracts)
└── Directory.Build.props   # Shared package metadata (IsPackable=True, authors, license, repo URLs)
UnitTests/                  # Single test project at ROOT — not under tests/
benchmarks/                 # BenchmarkDotNet project, not part of CI
Geopack.slnx                # Solution file
```

`src/Directory.Build.props` sets `IsPackable=True`, `PublishAot=True`, `IsAotCompatible=True` for all packages. Root `Directory.Build.props` sets `IsPackable=False` by default.

---

## Commands

```bash
dotnet restore
dotnet build                        # warnings-as-errors; must be clean
dotnet format --verify-no-changes   # run before committing; CI enforces this
dotnet test
dotnet pack --configuration Release
```

CI runs in this order: **restore → format → build → test → pack**.
Run `dotnet format` before `dotnet build` locally to avoid spurious failures.

### Focused test runs

```bash
dotnet test --filter "ClassName=GeopackTests"
dotnet test --filter "FullyQualifiedName~Recalc"
```

---

## Build Configuration

Source of truth is `Directory.Build.props` and `global.json`, not README or CI workflow.

- **Target frameworks:** `net8.0;net10.0` (multi-targeted)
- **SDK pin:** `10.0.100`, roll-forward `patch` (`global.json`) — CI currently uses 9.0.x (discrepancy in workflow, local dev requires .NET 10 SDK)
- **Nullable:** enabled; **TreatWarningsAsErrors:** enabled
- **PDB:** embedded in assemblies
- **SourceLink:** enabled for GitHub (`Microsoft.SourceLink.GitHub`); maps embedded PDBs to source

## Versioning

- The base version lives in **one place**: root `Directory.Build.props` → `<PackageBaseVersion>2.0.3</PackageBaseVersion>`.
- In CI the version is pinned via `-p:MinVerVersionOverride` (computed by the CI bash script from `PackageBaseVersion`): pre-release on non-`main` branches, stable on `main`/semver tags.
- **To bump the version, change `PackageBaseVersion` in root `Directory.Build.props` — nothing else.** Never set `<Version>` or `<PackageVersion>` in a `.csproj`.

---

## Code Style (`.editorconfig` enforced as errors)

- **`var` is banned** — always use explicit types (`var` triggers an error)
- Private/internal fields: `_camelCase` prefix (error)
- Constants: `PascalCase` (error)
- Static private/internal fields: `s_` prefix (suggestion, not error)
- Allman braces (`csharp_new_line_before_open_brace = all`)
- `using` directives outside namespace
- 4-space indent for C#; 2-space for XML/props/csproj

---

## Testing

- **Framework:** xUnit v3 + Shouldly only. **No Moq, no AutoFixture** — this repo has no mocking needs.
- **Test project path:** `UnitTests/` at the repo root (not `tests/UnitTests/`)
- **Assembly name:** `AuroraScienceHub.Geopack.UnitTests`

### Reference data

Tests validate against Fortran-generated `.dat` files compiled with Intel Fortran Compiler (`ifx`). These are embedded resources:

- Location: `UnitTests/Geopack/TestData/*.dat`
- Embedded as: `AuroraScienceHub.Geopack.UnitTests.Geopack.TestData.<FileName>.dat`
- Fortran sources for regeneration: `UnitTests/Geopack/FortranSource/`, `UnitTests/ExternalFieldModels/FortranSource/`

Tests use `TestDataFixture` (xUnit collection fixture) to load `.dat` data once per test run — loading is expensive, do not move tests outside the collection.

Precision constant for Geopack tests: `8E-12`. For ExternalFieldModels: `1E-13`.

### Test naming conventions

- Class: `{ClassUnderTest}Tests` (partial classes per method group, e.g., `GeopackTests.Recalc.cs`)
- Method: `{Method}_{Scenario}_{ExpectedBehavior}`

---

## NuGet Dependencies

All versions managed centrally in `Directory.Packages.props`. Never add `Version=` to individual `.csproj` files. When adding a dependency, justify it — BCL is preferred.

---

## CI/CD

`.github/workflows/dotnet.yml`:

| Trigger | Result |
|---|---|
| Push to any non-`main` branch | Pre-release → GitHub Packages |
| Push to `main` | `{base}.{runNumber}` → NuGet.org |
| Semver tag (`*.*.*`) | Stable `{base}` → NuGet.org |

Version is computed via bash script in CI using `MinVerVersionOverride`, not MinVer git-tag discovery. Do not rely on `git tag` for versioning locally.

---

## Public API Constraints

Published packages have downstream consumers. Treat any public API change as a potential breaking change:
- Prefer additive changes
- Never rename or remove public types/members without explicit approval
- All public types and members require XML doc comments
- Use `internal sealed` by default; expose only what consumers need

## Do's and Don'ts

### Do

- ✅ Use central package management (`Directory.Packages.props`) for all dependencies
- ✅ Follow existing patterns in the codebase
- ✅ Use XML doc comments on public API surface
- ✅ Use `internal` and `sealed` modifiers by default; only expose what consumers need
- ✅ Preserve public API compatibility for published packages unless a breaking change is explicitly requested
- ✅ Validate changes compile with warnings-as-errors enabled
- ✅ Use async/await for all I/O operations
- ✅ Write tests for new functionality before or alongside implementation
- ✅ Keep functions small and focused
- ✅ Keep packages focused — one responsibility per package
- ✅ Update the package changelog (`PackageReleaseNotes` in `src/Directory.Build.props`) once per branch/release with user-visible changes — not per commit or individual change

### Don't

- ❌ Suppress warnings with `#pragma` or `<NoWarn>` without justification
- ❌ Add `Version` attributes to individual `.csproj` files
- ❌ Modify `Directory.Build.props` or `Directory.Packages.props` without explicit request
- ❌ Add dependencies that duplicate BCL functionality
- ❌ Add dependencies without justification
- ❌ Leave empty catch blocks or swallow exceptions silently
- ❌ Use `dynamic` or reflection where generics/interfaces suffice
- ❌ Break public API contracts (this is a library — consumers depend on stability)
- ❌ Rename, remove, or materially change behavior of published public APIs without explicit approval
- ❌ Skip nullable annotations on public APIs

---

## Success Indicators

These principles are working if you see:

- Fewer unnecessary changes in diffs
- Fewer rewrites due to overcomplication
- Clarifying questions come before implementation (not after mistakes)
- Clean, minimal PRs without drive-by refactoring
- Tests that document expected behavior
