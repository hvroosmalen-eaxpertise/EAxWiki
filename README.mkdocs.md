Running the wiki locally with MkDocs

This repository contains a `wiki/` folder with Markdown content. Use MkDocs to serve and preview the wiki locally.

Quick start (PowerShell):

1. Ensure Python 3.x is installed and `python` is available on PATH.
2. In repo root, create a virtual environment and activate it:
   python -m venv .venv
   .\.venv\Scripts\Activate.ps1
3. Install dependencies:
   pip install -r requirements.txt
4. Serve the site:
   .\scripts\serve-mkdocs.ps1 -Port 8000

Or run `mkdocs serve` directly if you have MkDocs installed globally.

Notes:
- The mkdocs configuration uses `docs_dir: wiki`, so MkDocs will serve files from the `wiki/` directory.
- To regenerate the wiki from the EA repository using the .NET exporter, run the EAxWiki tool first so that `wiki/` contains up-to-date content.

Important note about the URL shown by MkDocs

When MkDocs starts it may show "Serving on http://0.0.0.0:8000". The address 0.0.0.0 is a listen address and is not usable in a browser. Use one of the following instead:

- http://localhost:8000
- http://127.0.0.1:8000
- http://<your-machine-ip>:8000  (if you need to access the server from another device and your firewall allows it)

CI / Deployment

A GitHub Actions workflow has been added at `.github/workflows/mkdocs-deploy.yml` that will build the MkDocs site and publish it to the `gh-pages` branch on pushes to `master`.

Notes about deployment:
- The workflow uses the built-in `GITHUB_TOKEN`. No additional secrets are required for a simple publish to `gh-pages`.
- The workflow publishes the generated `site/` directory. If you need a different branch or settings, edit the workflow accordingly.
