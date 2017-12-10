$(function () {
    var url = $('#departamentos2URL').data('request-url');
    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            for (var obj in r) {
                $('#departamento').append($('<option>', {
                    value: r[obj].idDepartamento,
                    text: r[obj].descripcion
                }));
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("No se encontro");
        }
    });
});