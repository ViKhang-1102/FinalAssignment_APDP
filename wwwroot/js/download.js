// Enhanced download helper
// Features:
// - Fetch-based download with error handling
// - Optional CSRF token header support
// - Integration with toasts.js (showSuccess/showError/showInfo) if available
// - Fallback to window.open
console.log('[download.js] loaded');

function _showInfo(msg) { if (typeof showInfo === 'function') showInfo(msg); else console.info(msg); }
function _showSuccess(msg) { if (typeof showSuccess === 'function') showSuccess(msg); else console.log(msg); }
function _showError(msg) { if (typeof showError === 'function') showError(msg); else console.error(msg); }

/**
 * Download a file using fetch and save via blob
 * options: { method, headers, csrfToken, csrfHeaderName, filename }
 */
window.downloadFile = async function (url, options = {}) {
    try {
        _showInfo('Starting download...');

        const method = (options.method || 'GET').toUpperCase();
        const headers = new Headers(options.headers || {});

        // Attach explicit CSRF token header when provided
        if (options.csrfToken) {
            const headerName = options.csrfHeaderName || 'RequestVerificationToken';
            headers.set(headerName, options.csrfToken);
        }

        const resp = await fetch(url, {
            method,
            headers,
            credentials: options.credentials || 'include'
        });

        if (!resp.ok) {
            const text = await resp.text().catch(() => null);
            const message = `Download failed: ${resp.status} ${resp.statusText}${text ? ' - ' + text : ''}`;
            _showError(message);
            throw new Error(message);
        }

        const blob = await resp.blob();

        // Determine filename: options.filename > Content-Disposition header > URL segment
        let filename = options.filename;
        if (!filename) {
            const cd = resp.headers.get('Content-Disposition');
            if (cd) {
                const m = /filename\*=UTF-8''([^;]+)|filename="?([^";]+)"?/.exec(cd);
                filename = (m && (m[1] || m[2])) ? decodeURIComponent(m[1] || m[2]) : null;
            }
        }
        if (!filename) {
            try { filename = url.split('/').pop().split('?')[0] || 'file'; } catch { filename = 'file'; }
        }

        const blobUrl = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = blobUrl;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        a.remove();
        URL.revokeObjectURL(blobUrl);

        _showSuccess('Download started');
        return true;
    }
    catch (err) {
        console.error('[download.js] downloadFile error', err);
        _showError('Download failed');
        // Fallback: try to open in new tab/window
        try {
            window.open(url, options.target || '_blank');
        } catch (e) {
            console.error('[download.js] fallback open failed', e);
        }
        return false;
    }
};

// Simple open helper (keeps existing usage)
window.downloadOpen = function (url, target = '_blank') {
    try {
        window.open(url, target);
    } catch (e) {
        console.error('[download.js] downloadOpen error', e);
    }
};

// Expose helper to read CSRF token from a meta tag if present
window.getCsrfTokenFromMeta = function (metaName = 'RequestVerificationToken') {
    try {
        const m = document.querySelector(`meta[name="${metaName}"]`);
        return m ? m.getAttribute('content') : null;
    } catch (e) {
        return null;
    }
};
