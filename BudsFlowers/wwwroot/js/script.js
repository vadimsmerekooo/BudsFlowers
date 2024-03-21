document.getElementById("navbar-toggler").addEventListener('click', ev => {
    const icon = document.getElementById('navbar-toggler-icon');
    const collapse = document.getElementById('navbarSupportedContent');
    if(icon.classList.contains("bi-list-nested") && !collapse.classList.contains("show")){
        icon.classList.remove('bi-list-nested');
        icon.classList.add("bi-x");
    }
    else{
        icon.classList.remove("bi-x");
        icon.classList.add('bi-list-nested');
    }
});
window.addEventListener('load', function () {
    const datatablesSimple = document.getElementById('datatablesSimple');
    if (datatablesSimple) {
        new simpleDatatables.DataTable(datatablesSimple, {
            labels: {
                placeholder: "Поиск...",
                searchTitle: "Поиск в таблице",
                perPage: "кол-во элементов",
                noRows: "Не найдено элементов",
                noResults: "Нет результатов, соответствующих вашему поисковому запросу",
                info: "Отображение записей от {start} до {end} из {rows}.",
            },
            perPage: 25
        });
    }
    new SmartPhoto(".js-img-viewer");
    new SmartPhoto(".js-img-viewer-fit");
    
});
$(function() {
    $('#pick-date').pickadate(); 
});