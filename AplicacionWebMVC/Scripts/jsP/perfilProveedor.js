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
            console.log(r);
            swal("Error!", "No se pudo cargar la informacion. Ocurrio el siguiente error: " + r.message, "error");
        }
    });
    $("#btnGuardarContraseña").on('click', function (e) {
        console.log("entro")
        var passwordA = document.getElementById('passwordA').value;
        var passwordN = document.getElementById('passwordN').value;
        var passwordN2 = document.getElementById('passwordN2').value;

        var d = {
            passwordA: passwordA,
            passwordN: passwordN,
            passwordN2: passwordN2
        };
        var url = $('#CambiarPasswordURL').data('request-url');
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(d),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                $(".loader").fadeIn(1);
            },
            complete: function () {

            },
            success: function (r) {
                $(".loader").fadeOut(100);
                if (r.code === "error") {
                    swal("Error!", "Ocurrio el siguiente error: " + r.message, "error");
                } else {
                    swal("Guardado!", "Se ha actualizado la contraseña", "success");
                }


            }, error: function (r) {
                $(".loader").fadeOut(100);
                swal("Error!", "Ocurrio el siguiente error: " + r.message, "error");
            }
        });
    });
    $("#btnGuardar").on('click', function (e)    {
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
            beforeSend: function () {
                $(".loader").fadeIn(1);
            },
            complete: function () {

            },
            success: function (r) {
                $(".loader").fadeOut(100);
                swal("Guardado!", "Se ha actualizado la informacion correctamente", "success");
            }, error: function (r) {
                $(".loader").fadeOut(100);
                swal("Error!", "Ocurrio el siguiente error: "+r.message, "error");
            }
        });
    });
    $("#btnGuardarArchivo").on('click', function (e) {
        var files = $("#file");
        var totalFiles = files[0].files.length;
        var data = new FormData();
        for (var i = 0; i < totalFiles; i++) {
            data.append("Foto", files[0].files[i]);
        }
        var fileUploadUrl = $('#UploadInfoProveedorURL').data('request-url');
        $.ajax({
            url: fileUploadUrl,
            data: data,
            type: 'POST',
            contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
            processData: false, // NEEDED
            beforeSend: function () {
                $(".loader").fadeIn(1);
            },
            complete: function () {

            },
            success: function (response) {
                $(".loader").fadeOut(100);
                document.getElementById("file").value = "";
                swal("Guardado!", "Se ha guardado la solicitud correctamente", "success");
            },
            error: function (error) {
                $(".loader").fadeOut(100);
                document.getElementById("file").value = "";
                swal("Error!", "Ocurrio el siguiente error: " + error.message, "error");
            }
        });
    });
});
