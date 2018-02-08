$(function () {
    var url = $('#GetProveedorURL').data('request-url');
    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (r) {
            console.log(r);
            document.getElementById('razSoc').value = r.razSoc2;
            document.getElementById('RFC').value = r.RFC;
            document.getElementById('direccion').value = r.direccion;
            document.getElementById('telefono').value = r.telefono;
            document.getElementById('representante').value = r.representante;
            document.getElementById('colonia').value = r.colonia;
            document.getElementById('ciudad').value = r.ciudad;
            document.getElementById('codigoPostal').value = r.codigoPostal;
            document.getElementById('fax').value = r.fax;
            document.getElementById('tipoProveedor').value = r.tipoProveedor;
        }, error: function (r) {
            console.log("No se encontro");
        }
    });
    $("#btnGuardar").on('click', function (e) {
        console.log(document.getElementById('razSoc').value);
        var d = {
            razSoc:document.getElementById('razSoc').value,
            RFC:document.getElementById('RFC').value,
            direccion:document.getElementById('direccion').value,
            telefono:document.getElementById('telefono').value,
            representante:document.getElementById('representante').value,
            colonia:document.getElementById('colonia').value,
            ciudad:document.getElementById('ciudad').value,
            codigoPostal:document.getElementById('codigoPostal').value,
            fax:document.getElementById('fax').value,
            tipoProveedor:document.getElementById('tipoProveedor').value
        }
        var url = $('#UpdateProveedorURL').data('request-url');
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(d),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {
                swal("Guardado!", "Se ha actualizado la informacion correctamente", "success");
            }, error: function (r) {
                swal("Error!", "Ocurrio el siguiente error: "+r.message, "error");
            }
        });
    });
});