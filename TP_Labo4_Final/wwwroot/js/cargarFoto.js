const { read } = require("@popperjs/core");

$(document).ready(function () {
    $("#seleccionImg").change(function () {
        var tam = this.files[0].size;
        if (tam > 50000000) {
            alert("El tamaño de la imagen es demasiado grande!");
        } else {
            readURL(this);
        }
    });
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#imagen").attr("src", e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
