# Publishing to PyPI

The Python package is published to PyPI as **`geopack-2008`** (import name
`geopack`) via **trusted publishing** (OpenID Connect). No PyPI token is stored
in the repository — the `python-package.yml` workflow authenticates with PyPI
through a pre-authorized "pending publisher".

## One-time PyPI setup

1. Create a [PyPI](https://pypi.org) account if you don't have one.
2. Under **Account settings → Trusted publishing → Add a new pending publisher**,
   fill in:

   | Field | Value |
   | --- | --- |
   | PyPI Project Name | `geopack-2008` |
   | Owner | `Aurora-Science-Hub` |
   | Repository name | `Geopack` |
   | Workflow name | `python-package.yml` |
   | Environment name | *(leave empty / "Any")* |

The project itself is created automatically by the first successful publish —
there is no separate "create project" step.

## Release process

1. Bump the version in **three places** (they must stay equal):
   - `Directory.Build.props` → `<PackageBaseVersion>`
   - `python/pyproject.toml` → `[project] version`
   - `python/geopack/__init__.py` → `__version__`
2. Add a `CHANGELOG.md` entry.
3. Commit, merge into `main`, and push.
4. On a push to `main`: `python-package.yml` publishes the new version to PyPI,
   `dotnet.yml` publishes to NuGet.org, and `release.yml` creates a GitHub
   Release (`vX.Y.Z`, notes from the matching CHANGELOG section). Bump the
   version to release again.

## Result

```bash
pip install geopack-2008
```

```python
import geopack
```
