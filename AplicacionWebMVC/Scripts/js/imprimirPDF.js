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
            if (element.attr("name") == "departamento") {
                $("#errDepartamento").text("*Es requerido");
            }
            if (element.attr("name") == "uso") {
                $("#errUso").text("*Es requerido");
            }
            if (element.attr("name") == "fechaNecesitar") {
                $("#errFechaN").text("*Es requerido");
            }
            if (element.attr("name") == "fechaActual") {
                $("#errFechaA").text("*Es requerido");
            }
            if (element.attr("name") == "ejercicio") {
                if (error.text().indexOf("number") != -1) {
                    $("#errEjercicio").text("*Debe ser año en numero");
                }
                if (error.text().indexOf("required") != -1) {
                    $("#errEjercicio").text("*Es requerido");
                }
            }
            if (element.attr("name") == "partidaPrep") {
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
                swal("¿Desea imprimir con detalle o sin detalle?", {
                    buttons: {
                        cancel: "Cancelar!",
                        defeat: {
                            text: "Con detalle",
                            value: "conDetalle",
                        },
                        catch: {
                            text: "Sin detalle",
                            value: "sinDetalle",
                        }

                    },
                })
                    .then((value) => {
                        switch (value) {
                            case "conDetalle":
                                imprimirConnDetalle();
                                break;
                            case "sinDetalle":
                                imprimirSinDetalle();
                                break;
                            default:
                                break;
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
    function imprimirSinDetalle() {
        var externalDataRetrievedFromServer = [];
        var i = 1;

        obj.forEach(function (value) {
            var row = [];
            externalDataRetrievedFromServer.push({
                Partida: i,
                Cantidad: value.Cantidad,
                Unidad: value.Unidad,
                'Descripcion del articulo': value.Descripcion,
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
                                    [{ image:img, width: 45, height: 45, rowSpan: 3, style: 'tableHeader', alignment: 'center' },
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
                            margin: [480, 10, 0, 0],
                            table: {
                                widths: ['auto', 100,100],
                                headerRows: 2,
                                body: [
                                    [{ text: 'Fecha', style: 'tableHeader', alignment: 'center' }, { text: 'Numero', style: 'tableHeader', alignment: 'center' }, { text: 'Pre-Requisición', style: 'tableHeader', alignment: 'center' }],
                                    ['' + document.getElementById('fechaActual').value, '', { text: prerequisicionG, alignment:'center'}]
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
                    margin: [-110, 1, 0, 0],
                    table: {
                        widths: ['95.1%', '12.3%', '10%'],
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
                        widths: ['59%', '58%'],
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


            ], pageSize: 'LETTER',
            pageMargins: 120,
            styles: {
                personal: {
                    fontSize: 9,
                }
            }
        };
        pdfMake.createPdf(docDefinition).open();
    }
    function imprimirConnDetalle() {
        var externalDataRetrievedFromServer = [], detallesTable = [];
        var c1='\n', c2='\n', c3='\n', c4='\n', c5='\n', c6='\n', c7='\n', c8='\n', c9='\n';
        var i = 1;
        if (chkBox.trabajoSindicato == true) {
            c1 = 'X';
        }
        if (chkBox.retencionImpuesto == true) {
            c2 = 'X';
        }
        if (chkBox.altura == true) {
            c3 = 'X';
        }
        if (chkBox.espaciosConfinados == true) {
            c4 = 'X';
        }
        if (chkBox.electrico == true) {
            c5 = 'X';
        }
        if (chkBox.corte == true) {
            c6 = 'X';
        }
        if (chkBox.soldadura == true) {
            c7 = 'X';
        }
        if (chkBox.izajes == true) {
            c8 = 'X';
        }
        if (chkBox.montacarga == true) {
            c9 = 'X';
        }
        obj.forEach(function (value) {
            var row = [];
            externalDataRetrievedFromServer.push({
                Partida: i,
                Cantidad: value.Cantidad,
                Unidad: value.Unidad,
                'Descripcion del articulo': value.Descripcion,
                Clave: value.Clave, Existencia: value.Existencia,
                'Precio unitario': value.PrecioU,
                'Precio total': parseFloat(value.PrecioU) * parseFloat(value.Cantidad)
            })
            detallesTable.push({
                Partida: i,
                Detalle: value.Detalle
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
                if (page === 1) {
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
                                margin: [480, 10, 0, 0],
                                table: {
                                    widths: ['auto', 100, 100],
                                    headerRows: 2,
                                    body: [
                                        [{ text: 'Fecha', style: 'tableHeader', alignment: 'center' }, { text: 'Numero', style: 'tableHeader', alignment: 'center' }, { text: 'Pre-Requisición', style: 'tableHeader', alignment: 'center' }],
                                        ['' + document.getElementById('fechaActual').value, '', { text: prerequisicionG, alignment: 'center' }]
                                    ]
                                }
                            }
                        ],
                        margin: [10, 10]
                    };
                } else {
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
                                        ['', { colSpan: 4, rowSpan: 1, alignment: 'center', text: 'SOLICITUD Y REQUISICION DE BIENES Y/O SERVICIOS (ANEXO)' }, '', '', '']
                                    ]
                                }
                            }
                        ],
                        margin: [10, 10]
                    };
                }
                
            },
            footer: function (page, pages) {
                if (page === 1) {
                    return {
                        columns: [
                            {
                                margin: [0, 0, 0, 0],
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
                } else {
                    return { columns: [] };
                }
            },
            content: [
                table(externalDataRetrievedFromServer, [{ text: 'Partida', alignment: 'center' }, { text: 'Cantidad', alignment: 'center' }, { text: 'Unidad', alignment: 'center' }, { text: 'Descripcion del articulo', alignment: 'center' }, { text: 'Clave', alignment: 'center' }, { text: 'Existencia', alignment: 'center' }, { text: 'Precio unitario', alignment: 'center' }, { text: 'Precio total', alignment: 'center' }]),
                {
                    margin: [-110, 1, 0, 0],
                    table: {
                        widths: ['95.1%', '12.3%', '10%'],
                        body: [
                            [{ text: '', alignment: 'center' },
                            { text: 'Total', alignment: 'center' },
                            { text: '$ ' + total, style: 'tableHeader', alignment: 'center'}]
                        ]
                    }
                },
                {
                    margin: [-110, 2, 0, 0],
                    table: {
                        widths: ['59%', '58%'],
                        headerRows: 2,
                        body: [
                            [{ text: 'Departamento: ' + document.getElementById("departamento").value, style: 'tableHeader', alignment: 'left' },
                            { text: 'Observaciones', style: 'tableHeader', alignment: 'center' }],
                            [{ text: 'Uso: ' + document.getElementById("uso").value, style: 'tableHeader', alignment: 'left' }, { text: document.getElementById("observaciones").value.replace(/[\r\n\v]+/g, ' '), style: 'personal', alignment: 'justify', rowSpan: 3 }],
                            [{ text: 'Fecha a necesitar: ' + document.getElementById("fechaNecesitar").value, style: 'tableHeader', alignment: 'left' }, ''],
                            [{ text: 'Partida presupuestal: ' + document.getElementById("partidaPrep").value, style: 'tableHeader', alignment: 'left' }, '']
                        ]
                    }, pageBreak: 'after'
                },
                tableDetalle(detallesTable, [{ text: 'Partida', alignment: 'center' }, { text: 'Detalle', alignment: 'center' }]),
                {

                    margin: [-110, 2, 0, 0],
                    table: {
                        widths: [100, 108, 'auto', 108, 'auto', 'auto', 'auto', 108, 105],
                        headerRows: 2,
                        body: [
                            [{ text: 'Trabajo sindicato', style: 'tableHeader', alignment: 'center' }, { text: 'Retencion impuesto 2.5%', style: 'tableHeader', alignment: 'center' },
                            { text: 'Altura', style: 'tableHeader', alignment: 'center' }, { text: 'Espacios confinados', style: 'tableHeader', alignment: 'center' },
                            { text: 'Electrico', style: 'tableHeader', alignment: 'center' }, { text: 'Corte', style: 'tableHeader', alignment: 'center' },
                            { text: 'Soldadura', style: 'tableHeader', alignment: 'center' }, { text: 'Izajes de carga', style: 'tableHeader', alignment: 'center' }, { text: 'Operaciones montacarga', style: 'tableHeader', alignment: 'center' }],
                            [{ text: c1, alignment: 'center' }, { text: c2, alignment: 'center' }, { text: c3, alignment: 'center' },
                                { text: c4, alignment: 'center' }, { text: c5, alignment: 'center' }, { text: c6, alignment: 'center' },
                                { text: c7, alignment: 'center' }, { text: c8, alignment: 'center' }, { text: c9, alignment: 'center' }]
                        ]
                    }
                },


            ], pageSize: 'LETTER',
            pageMargins: [120,120,120,80],
            styles: {
                personal: {
                    fontSize: 9,
                }
            }
        };
        pdfMake.createPdf(docDefinition).open();
    }
    function buildTableBody(data, columns) {
        var body = [];
        body.push(columns);

        data.forEach(function (row) {
            var dataRow = [];

            columns.forEach(function (column) {
                if (column.text =="Descripcion del articulo") {
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
    function tableDetalle(data, columns) {
        return {
            margin: [-110, -40, -110, 0],
            table: {
                headerRows: 1,
                widths: [39, '*'],
                body: buildTableBody(data, columns)
            }
        };
    }
});