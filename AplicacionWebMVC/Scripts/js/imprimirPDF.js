$(document).ready(function () {
    var img;
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "/WebAdquisicion/file/logo.png", true);
    //xhr.open("GET", "/file/logo.png", true);
    xhr.responseType = "blob";
    xhr.onload = function (e) {
        var reader = new FileReader();
        reader.onload = function (event) {
            var res = event.target.result;
            img = res;
        }
        var file = this.response;
        reader.readAsDataURL(file)
    };
    xhr.send()
    
    $("form[name='infoSol']").validate({
        rules: {
            departamento: "required",
            uso: "required",
            fechaNecesitar: "required",
            fechaActual: "required",
            ejercicio: { required: true, number: true },
            partidaPrep: "required"
        },
        errorPlacement: function (error, element) {
            //Custom position: first name
            if (element.attr("name") === "departamento") {
                $("#errDepartamento").text("*Es requerido");
            }
            if (element.attr("name") === "uso") {
                $("#errUso").text("*Es requerido");
            }
            if (element.attr("name") === "fechaNecesitar") {
                $("#errFechaN").text("*Es requerido");
            }
            if (element.attr("name") === "fechaActual") {
                $("#errFechaA").text("*Es requerido");
            }
            if (element.attr("name") === "ejercicio") {
                if (error.text().indexOf("number") !== -1) {
                    $("#errEjercicio").text("*Debe ser año en numero");
                }
                if (error.text().indexOf("required") !== -1) {
                    $("#errEjercicio").text("*Es requerido");
                }
            }
            if (element.attr("name") === "partidaPrep") {
                $("#errPartidaPrep").text("*Es requerido");
            }
            // Default position: if no match is met (other fields)
            else {
                error.append($('.errorTxt span'));
            }
        },
        // Specify validation error messages
        messages: {

        }
    });
    var externalDataRetrievedFromServer;
    $(document).on('click', '#btnImprimir', function (e) {
        reestablecerInputsError();
        if ($("form[name='infoSol']").valid()) {
            if (!$.isEmptyObject(obj)) {
                swal({
                    title: "¿Desea imprimir?",
                    text: "",
                    icon: "info",
                    buttons: true,
                })
                    .then((willDelete) => {
                        if (willDelete) {
                            imprimir();
                        }
                    });
            } else {
                $("#errTablaP").text("*Se necesita al menos una partida");
                swal("Faltan datos", "Se necesita al menos una partida!", "error");
            }
        } else {
            swal("Faltan datos", "Debe llenar los campos requeridos para poder imprimir!", "error");
        } 
    });
    function imprimir() {
        var externalDataRetrievedFromServer = [];
        
        var i = 1;
        obj.forEach(function (value) {
            var row = [];
            externalDataRetrievedFromServer.push({
                Partida: i,
                Cantidad: value.Cantidad,
                Unidad: value.Unidad,
                'Descripcion del articulo': value.Descripcion + ':\n' + value.Detalle,
                Clave: value.Clave, Existencia: value.Existencia,
                'Precio unitario': value.PrecioU,
                'Precio total': parseFloat(value.PrecioU) * parseFloat(value.Cantidad)
            })
            i++;
        });
        for (i; i <= 8; i++) {
            externalDataRetrievedFromServer.push({
                Partida: i,
                Cantidad: "",
                Unidad: "",
                'Descripcion del articulo': "",
                Clave: "",
                Existencia: "",
                'Precio unitario': "",
                'Precio total': ""
            })
        }
        var docDefinition = {
            pageSize: 'A4',
            pageOrientation: 'landscape',
            header: function (page, pages) {
                return {
                    stack: [
                        {
                            table: {
                                widths: [45, '*', '*', '*', '*'],
                                headerRows: 1,
                                body: [
                                    [{ image: img, width: 45, height: 45, rowSpan: 3, style: 'tableHeader', alignment: 'center' },
                                    { text: 'Identificacion', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Version', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Fecha', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Paginas', style: 'tableHeader', alignment: 'center' }],
                                    ['', { text: identificacion, style: 'tableHeader', alignment: 'center' },
                                        { text: version, style: 'tableHeader', alignment: 'center' },
                                        { text: fecha, style: 'tableHeader', alignment: 'center' },
                                        { text: page + " de " + pages, style: 'tableHeader', alignment: 'center' }],
                                    ['', { colSpan: 4, rowSpan: 1, alignment: 'center', text: 'SOLICITUD Y REQUISICION DE BIENES Y/O SERVICIOS' }, '', '', '']
                                ]
                            }
                        },
                        {
                            margin: [530, 10, 0, 0],
                            table: {
                                widths: ['auto', 100, 100],
                                headerRows: 2,
                                body: [
                                    [{ text: 'Fecha', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Numero', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Pre-Requisición', style: 'tableHeader', alignment: 'center' }],
                                    ['' + document.getElementById('fechaActual').value, '',
                                    { text: ''+prerequisicionG, alignment: 'center' }]
                                ]
                            }
                        }
                    ],
                    margin: [10, 10]
                };
            },
            footer: function (page, pages) {
                return {
                    columns: [
                        {
                            margin: [0, 30, 0, 0],
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*'],
                                headerRows: 2,
                                body: [
                                    [{ text: 'Departamento Solicitante', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Almacen', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Superintendente', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Director general', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Compras', style: 'tableHeader', alignment: 'center' },
                                    { text: 'Proveedor', style: 'tableHeader', alignment: 'center' }],
                                    ['\n\n\n', '', '', '', '', '']
                                ]
                            }
                        }
                    ],
                    margin: [10, 10]
                };
            },
            content: [
                table(externalDataRetrievedFromServer, [{ text: 'Partida', alignment: 'center' }, { text: 'Cantidad', alignment: 'center' }, { text: 'Unidad', alignment: 'center' }, { text: 'Descripcion del articulo', alignment: 'center' }, { text: 'Clave', alignment: 'center' }, { text: 'Existencia', alignment: 'center' }, { text: 'Precio unitario', alignment: 'center' }, { text: 'Precio total', alignment: 'center' }]),
                {
                    margin: [-110, 2, 0, 0],
                    table: {
                        widths: ['95.5%', '11.5%', '9%'],
                        body: [
                            [{ text: '', alignment: 'center' },
                            { text: 'Total', alignment: 'center' },
                            { text: '$ ' + total, style: 'tableHeader', alignment: 'center' }]
                        ]
                    }
                },
                {
                    margin: [-110, 2, 0, 0],
                    table: {
                        widths: ['59%', '57%'],
                        headerRows: 2,
                        body: [
                            [{ text: 'Departamento: ' + document.getElementById("departamento").value, style: 'tableHeader', alignment: 'left' },
                            { text: 'Observaciones', style: 'tableHeader', alignment: 'center' }],
                            [{ text: 'Uso: ' + document.getElementById("uso").value, style: 'tableHeader', alignment: 'left' }, { text: document.getElementById("observaciones").value.replace(/[\r\n\v]+/g, ' '), style: 'personal', alignment: 'justify', rowSpan: 3 }],
                            [{ text: 'Fecha a necesitar: ' + document.getElementById("fechaNecesitar").value, style: 'tableHeader', alignment: 'left' }, ''],
                            [{ text: 'Partida presupuestal: ' + document.getElementById("partidaPrep").value, style: 'tableHeader', alignment: 'left' }, '']
                        ]
                    }
                }
            ],
            pageMargins: 120,
            styles: {
                personal: {
                    fontSize: 9,
                }
            }};
        pdfMake.createPdf(docDefinition).open();
    }
});
function ToJavaScriptDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    if (dt.getDate() < 10) {
        dia = "0" + dt.getDate();
    } else {
        dia = dt.getDate();
    }
    if ((dt.getMonth() + 1) < 10) {
        mes = "0" + (dt.getMonth() + 1);
    } else {
        mes = (dt.getMonth() + 1);
    }
    return (dia + "/" + mes + "/" + dt.getFullYear());
}
function ImprimirPDF(preReq, depa, ejer) {
    var totalL = 0;
    var d={
        "preRequisicion": preReq,
        "departamento": depa,
        "ejercicio":ejer
    }
    var url = $('#imprimirURL').data('request-url');
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(d),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            console.log(result);
            var identificacion = "no tiene", version = 0, fecha = "01/01/2017";
            var img;
            var externalDataRetrievedFromServer = [];
            var i = 1;
            var xhr = new XMLHttpRequest();
            var f = new Date();
            var fechaActual = f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear();
            xhr.open("GET", "/WebAdquisicion/file/logo.png", true);
            //xhr.open("GET", "/file/logo.png", true);
            xhr.responseType = "blob";
            
            xhr.onload = function (e) {
                console.log(e);
                var reader = new FileReader();
                reader.onload = function (event) {
                    var res = event.target.result;
                    img = res;
                    var url = $('#ejercicioURL').data('request-url');
                    $.ajax({
                        type: "GET",
                        url: url,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        cache: false,
                        success: function (result) {
                            identificacion = result.ejercicio.identificacion;
                            fecha = ToJavaScriptDate(result.ejercicio.fecha);
                            version = result.ejercicio.version;
                        }, error: function () {

                        }
                    });
                    result.partidas.forEach(function (value) {
                        var row = [];
                        externalDataRetrievedFromServer.push({
                            Partida: i,
                            Cantidad: value.cantidad,
                            Unidad: value.unidad,
                            'Descripcion del articulo': value.descripcion + ':\n' + value.detalle,
                            Clave: value.material,
                            Existencia: value.existencia,
                            'Precio unitario': value.costoU,
                            'Precio total': parseFloat(value.costoU) * parseFloat(value.cantidad)
                        })
                        totalL = totalL + parseFloat(value.costoU) * parseFloat(value.cantidad);
                        i++;
                    });
                    for (i; i <= 8; i++) {
                        externalDataRetrievedFromServer.push({
                            Partida: i,
                            Cantidad: "",
                            Unidad: "",
                            'Descripcion del articulo': "",
                            Clave: "",
                            Existencia: "",
                            'Precio unitario': "",
                            'Precio total': ""
                        })
                    }
                    var docDefinition = {
                        pageSize: 'A4',
                        pageOrientation: 'landscape',
                        header: function (page, pages) {
                            return {
                                stack: [
                                    {
                                        table: {
                                            widths: [45, '*', '*', '*', '*'],
                                            headerRows: 1,
                                            body: [
                                                [{ image: img, width: 45, height: 45, rowSpan: 3, style: 'tableHeader', alignment: 'center' },
                                                { text: 'Identificacion', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Version', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Fecha', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Paginas', style: 'tableHeader', alignment: 'center' }],
                                                ['', { text: identificacion, style: 'tableHeader', alignment: 'center' },
                                                    { text: version, style: 'tableHeader', alignment: 'center' },
                                                    { text: fecha, style: 'tableHeader', alignment: 'center' },
                                                    { text: page + " de " + pages, style: 'tableHeader', alignment: 'center' }],
                                                ['', { colSpan: 4, rowSpan: 1, alignment: 'center', text: 'SOLICITUD Y REQUISICION DE BIENES Y/O SERVICIOS' }, '', '', '']
                                            ]
                                        }
                                    },
                                    {
                                        margin: [530, 10, 0, 0],
                                        table: {
                                            widths: ['auto', 100, 100],
                                            headerRows: 2,
                                            body: [
                                                [{ text: 'Fecha', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Numero', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Pre-Requisición', style: 'tableHeader', alignment: 'center' }],
                                                ['' + fechaActual, '',
                                                { text: '' + result.solicitud.preRequisicion, alignment: 'center' }]
                                            ]
                                        }
                                    }
                                ],
                                margin: [10, 10]
                            };
                        },
                        footer: function (page, pages) {
                            return {
                                columns: [
                                    {
                                        margin: [0, 30, 0, 0],
                                        table: {
                                            widths: ['*', '*', '*', '*', '*', '*'],
                                            headerRows: 2,
                                            body: [
                                                [{ text: 'Departamento Solicitante', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Almacen', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Superintendente', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Director general', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Compras', style: 'tableHeader', alignment: 'center' },
                                                { text: 'Proveedor', style: 'tableHeader', alignment: 'center' }],
                                                ['\n\n\n', '', '', '', '', '']
                                            ]
                                        }
                                    }
                                ],
                                margin: [10, 10]
                            };
                        },
                        content: [
                            table(externalDataRetrievedFromServer, [{ text: 'Partida', alignment: 'center' }, { text: 'Cantidad', alignment: 'center' }, { text: 'Unidad', alignment: 'center' }, { text: 'Descripcion del articulo', alignment: 'center' }, { text: 'Clave', alignment: 'center' }, { text: 'Existencia', alignment: 'center' }, { text: 'Precio unitario', alignment: 'center' }, { text: 'Precio total', alignment: 'center' }]),
                            {
                                margin: [-110, 2, 0, 0],
                                table: {
                                    widths: ['95.5%', '11.5%', '9%'],
                                    body: [
                                        [{ text: '', alignment: 'center' },
                                        { text: 'Total', alignment: 'center' },
                                        { text: '$ ' + totalL, style: 'tableHeader', alignment: 'center' }]
                                    ]
                                }
                            },
                            {
                                margin: [-110, 2, 0, 0],
                                table: {
                                    widths: ['59%', '57%'],
                                    headerRows: 2,
                                    body: [
                                        [{ text: 'Departamento: ' + result.solicitud.departamento, style: 'tableHeader', alignment: 'left' },
                                        { text: 'Observaciones', style: 'tableHeader', alignment: 'center' }],
                                        [{ text: 'Uso: ' + result.solicitud.uso, style: 'tableHeader', alignment: 'left' }, { text: result.solicitud.observaciones.replace(/[\r\n\v]+/g, ' '), style: 'personal', alignment: 'justify', rowSpan: 3 }],
                                        [{ text: 'Fecha a necesitar: ' + ToJavaScriptDate(result.solicitud.fechaNecesitar), style: 'tableHeader', alignment: 'left' }, ''],
                                        [{ text: 'Partida presupuestal: PARTIDA PRESUPUESTAL', style: 'tableHeader', alignment: 'left' }, '']
                                    ]
                                }
                            }
                        ],
                        pageMargins: 120,
                        styles: {
                            personal: {
                                fontSize: 9,
                            }
                        }
                    };
                    pdfMake.createPdf(docDefinition).open();
                }
                var file = this.response;
                reader.readAsDataURL(file)
            };
            xhr.send()
            
        }, error: function (result) {
            console.log(result);
            swal("Error!", "Ocurrio el siguiente error: " + result.message, "error");
        }
    });
}
function buildTableBody(data, columns) {
    var body = [];
    body.push(columns);

    data.forEach(function (row) {
        var dataRow = [];

        columns.forEach(function (column) {
            if (column.text === "Descripcion del articulo") {
                dataRow.push({ text: row[column.text].toString(), alignment: 'justify' });
            } else {
                dataRow.push({ text: row[column.text].toString(), alignment: 'center' });
            }

        })

        body.push(dataRow);
    });

    return body;
}
function table(data, columns) {
    return {
        margin: [-110, 5, -110, 0],
        table: {
            headerRows: 1,
            widths: [39, 48, 45, '*', 43, 58, 78, 63],
            body: buildTableBody(data, columns)
        }
    };
}