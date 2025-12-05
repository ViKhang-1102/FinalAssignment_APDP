// Minimal shim to avoid 404s and provide a tiny select2 interop surface
window.select2Interop = window.select2Interop || {};

window.select2Interop.init = function (selector) {
    try {
        var el = selector ? document.querySelector(selector) : null;
        if (!el) return false;
        // No-op init; actual select2 initialization is in select2-students.js
        return true;
    }
    catch (e) {
        console.error('select2Interop.init error', e);
        return false;
    }
};

window.select2Interop.setSelected = function (selector, items) {
    try {
        var el = selector ? document.querySelector(selector) : null;
        if (!el) return false;
        // No-op: select2-students.js exposes its own helpers
        return true;
    }
    catch (e) {
        console.error('select2Interop.setSelected error', e);
        return false;
    }
};

window.select2Interop.getSelected = function (selector) {
    try {
        var el = selector ? document.querySelector(selector) : null;
        if (!el) return [];
        var val = el.value;
        return val ? (Array.isArray(val) ? val : [val]) : [];
    }
    catch (e) {
        console.error('select2Interop.getSelected error', e);
        return [];
    }
};
