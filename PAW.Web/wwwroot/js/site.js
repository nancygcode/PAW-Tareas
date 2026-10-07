// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Reusable behaviour for all the CRUD screens ------------------------------------------------

document.addEventListener('click', async function (e) {

    // Details: loads a partial view into the shared modal (no new page is loaded)
    const details = e.target.closest('.js-details');
    if (details) {
        e.preventDefault();

        const modalEl = document.getElementById('detailsModal');
        const body = modalEl.querySelector('.modal-body');
        body.innerHTML = '<div class="text-center py-4"><div class="spinner-border" role="status"></div></div>';
        bootstrap.Modal.getOrCreateInstance(modalEl).show();

        try {
            const response = await fetch(details.dataset.url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } });
            if (!response.ok) throw new Error(response.status);
            body.innerHTML = await response.text();
        } catch (err) {
            body.innerHTML = '<div class="alert alert-danger mb-0">The details could not be loaded.</div>';
        }
        return;
    }

    // Delete: confirmation + POST (with anti-forgery token) through the hidden form
    const del = e.target.closest('.js-delete');
    if (del) {
        e.preventDefault();

        if (confirm(del.dataset.confirm || 'Do you want to delete this record?')) {
            const form = document.getElementById('deleteForm');
            form.action = del.dataset.url;
            form.submit();
        }
    }
});
