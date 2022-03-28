// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Кнопка прокрутки к началу страницы
const scrollTop = document.getElementById('scroll-top');

document.addEventListener('scroll', () => {
    scrollTop.hidden = window.scrollY < document.documentElement.clientHeight;
})

scrollTop.addEventListener('click', () => {
    window.scrollTo(0, 0);
    console.log("cdsc");
})

//Реализация поиска

const search = document.getElementById('search-box');
const articles = document.querySelectorAll('.content-card');
const match = document.querySelector('#matching-title');

search.addEventListener('input', (event) => {
    match.hidden = false;
    articles.forEach(article => {
        const title = article.querySelector('.title-card').textContent;
        if (title.toLowerCase().indexOf(event.target.value.toLowerCase()) > -1) {
            article.style.display = 'block';
            match.hidden = true;
        } else {
            article.style.display = 'none';
        }
    });
});

//Реализация копирования
document.oncopy = function () {
    let copypaste = "<p>Источник: <a href='" + document.location.href + "'>" + document.location.href +
        "</a> Любое использование материалов допускается только при наличии ссылки на сайт</p>";
    let selection = window.getSelection();
    const newDiv = document.createElement('div');
    const body = document.querySelector('body');
    body.appendChild(newDiv);
    newDiv.innerHTML = selection + copypaste;
    selection.selectAllChildren(newDiv);
    console.log(selection);
    window.setTimeout(function () {
        body.removeChild(newDiv);
    }, 0);
}

//slider

