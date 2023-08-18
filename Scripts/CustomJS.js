window.setTimeout(function () {
    $(".alert-success").fadeTo(500, 0).slideUp(500, function () {
        $(this).remove();
    });
}, 3000);


// Function to set the active menu item in localStorage
function setActiveMenu(menuId) {
    localStorage.setItem('activeMenu', menuId);
}

// On page load, check localStorage for active menu and apply active class
document.addEventListener('DOMContentLoaded', function () {
    var activeMenu = localStorage.getItem('activeMenu');
    if (activeMenu) {
        var menuItem = document.getElementById(activeMenu);
        if (menuItem) {
            menuItem.classList.add('active');
        }
    }
});


