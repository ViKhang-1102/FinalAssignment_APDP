function showToast(message, title = 'Notification', type = 'info') {
    const toastContainer = document.querySelector('.toast-container');
    if (!toastContainer) return;

    const typeClassMap = {
        success: 'text-bg-success',
        error: 'text-bg-danger',
        danger: 'text-bg-danger',
        warning: 'text-bg-warning',
        info: 'text-bg-info'
    };

    const headerClass = typeClassMap[type] || 'text-bg-secondary';

    const toastTemplate = `
        <div class="toast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-delay="4000">
            <div class="toast-header ${headerClass}">
                <strong class="me-auto">${title}</strong>
                <small>Just now</small>
                <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `;

    const wrapper = document.createElement('div');
    wrapper.innerHTML = toastTemplate.trim();
    const toastEl = wrapper.firstChild;
    toastContainer.appendChild(toastEl);

    const toast = new bootstrap.Toast(toastEl);
    toast.show();
}

// Convenience helpers
function showSuccess(message, title = 'Thành công') { showToast(message, title, 'success'); }
function showError(message, title = 'L?i') { showToast(message, title, 'error'); }
function showWarning(message, title = 'C?nh báo') { showToast(message, title, 'warning'); }
function showInfo(message, title = 'Thông báo') { showToast(message, title, 'info'); }