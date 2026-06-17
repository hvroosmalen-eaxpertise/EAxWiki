EAxWiki — Run the wiki locally with MkDocs

This repository contains a `wiki/` folder with Markdown content. Use MkDocs to serve and preview the wiki locally.

Important note about the URL shown by MkDocs

When MkDocs starts it may show "Serving on http://0.0.0.0:8000". The address 0.0.0.0 is a listen address and is not usable in a browser. Use one of the following instead:

- http://localhost:8000
- http://127.0.0.1:8000
- http://<your-machine-ip>:8000  (if you need to access the server from another device and your firewall allows it)

Quick start (PowerShell):

1. Ensure Python 3.x is installed and `python` is available on PATH.
2. In repo root, create a virtual environment and activate it:
   python -m venv .venv
   .\.venv\Scripts\Activate.ps1
3. Install dependencies:
   pip install -r requirements.txt
4. Serve the site:
   .\scripts\serve-mkdocs.ps1 -Port 8000

Troubleshooting

- If the browser can't connect but `localhost` works on the host machine, check Windows Firewall inbound rules for the port used (8000 by default).
- If you need the server to only be accessible locally, change the script to call `mkdocs serve --dev-addr 127.0.0.1:8000` or run `mkdocs serve` without `--dev-addr`.

CI / Deployment

A GitHub Actions workflow is present at `.github/workflows/mkdocs-deploy.yml` that builds the site and publishes it to the `gh-pages` branch on pushes to `master`.


