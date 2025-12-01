const toastTimings = {
    success: 3500,
    info: 3500,
    warning: 4500,
    error: 5000,
    danger: 5000
};

function showToast(message, title = 'Notification', type = 'info') {
    const toastContainer = document.querySelector('.toast-container');
    if (!toastContainer) return;

    const normalizedType = toastTimings[type] ? type : 'info';
    const toast = document.createElement('div');
    toast.className = 'tw-toast';
    toast.dataset.type = normalizedType;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    toast.innerHTML = `
        <span class="tw-toast__accent" aria-hidden="true"></span>
        <div class="tw-toast__body">
            <div class="tw-toast__header">
                <span class="tw-toast__title">${title}</span>
                <button type="button" class="tw-toast__close" aria-label="Close">&times;</button>
            </div>
            <p class="tw-toast__message">${message}</p>
        </div>`;

    toastContainer.appendChild(toast);

    requestAnimationFrame(() => {
        toast.classList.add('tw-toast--open');
    });

    let dismissed = false;
    const removeToast = () => {
        if (dismissed) return;
        dismissed = true;
        toast.classList.remove('tw-toast--open');
        toast.addEventListener('transitionend', () => toast.remove(), { once: true });
    };

    const closeButton = toast.querySelector('.tw-toast__close');
    closeButton?.addEventListener('click', removeToast);

    const timeout = toastTimings[normalizedType] ?? 4000;
    setTimeout(removeToast, timeout);
}

// Convenience helpers
function showSuccess(message, title = 'Successfully') { showToast(message, title, 'success'); }
function showError(message, title = 'Error') { showToast(message, title, 'error'); }
function showWarning(message, title = 'Warning') { showToast(message, title, 'warning'); }
function showInfo(message, title = 'Information') { showToast(message, title, 'info'); }