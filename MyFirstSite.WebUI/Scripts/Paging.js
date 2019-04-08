var elParent = document.querySelector('div.pagingParent');
if (elParent.addEventListener) {
    elParent.addEventListener('click', AddLightCurrentPagingNumber, false);
} else {
    elParent.attachEvent('onclick', AddLightCurrentPagingNumber);
}

function AddLightCurrentPagingNumber(e) {
    if (!e) {
        e = window.event;
    }
    var elementsLink = document.querySelectorAll('a.pagingNumber')
    for (var i = 0; i < elementsLink.length; i++) {
        el = elementsLink[i];
        var classString = 'pagingNumber btn btn-'
        el.className = (el.textContent == e.target.textContent) ? (classString + 'info') : (classString + 'default');
    }
}