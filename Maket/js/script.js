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
window.addEventListener('load',function(){
    new SmartPhoto(".js-img-viewer");
    new SmartPhoto(".js-img-viewer-fit");
});