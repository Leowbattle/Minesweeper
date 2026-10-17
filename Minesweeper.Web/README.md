# Minesweeper.Web

A browser build of Minesweeper using [KNI](https://github.com/kniEngine/kni) and
Blazor WebAssembly. The game itself is the C# source in `../Minesweeper`, linked
into this project and compiled with the `BLAZORGL` symbol defined.

## Run

```bash
dotnet run --project Minesweeper.Web
```

Then open <http://localhost:5210>.

## Publish

```bash
dotnet publish Minesweeper.Web -c Release
```

The published files are in `Minesweeper.Web/bin/Release/net10.0/publish/wwwroot`.

Use a Release build for normal play: Debug builds intentionally reveal mines
because `Board.cs` contains a `#if DEBUG` block.

## How it works

- KNI provides the MonoGame/XNA-compatible WebGL, WebAudio, and input backends.
- Blazor WebAssembly hosts the .NET runtime and drives the game loop through
  `requestAnimationFrame`.
- The game is rendered at a fixed 1024x1024 logical resolution and scaled
  uniformly into the browser window, with black letterbox/pillarbox bars.
- Mouse coordinates are translated from window pixels into the letterboxed
  logical frame before the game sees them.

## GitHub Pages

The workflow in `.github/workflows/pages.yml` builds the web app on pull requests
and deploys it to GitHub Pages when `master` is updated.

Before the first deploy, enable Pages in the repository settings:

1. Go to **Settings → Pages**.
2. Under **Build and deployment**, set **Source** to **GitHub Actions**.

The workflow rewrites the `<base href>` in `index.html` to the repository path
(for example `/Minesweeper/`) and adds a `.nojekyll` file so the `_framework`
directory is served. If you use a custom domain or a `<user>.github.io`
repository, adjust the base path logic in the workflow.

## Content

`wwwroot/Content/*.xnb` are the compiled content files from the repository's
`Release.zip`. The source assets and license are in `../Minesweeper/Content`.

To rebuild the content, use KNI's content builder for the `BlazorGL` platform.
The KNI content builder is easiest to run on Windows; its Linux tooling has
native dependencies that are not bundled with the NuGet package.
