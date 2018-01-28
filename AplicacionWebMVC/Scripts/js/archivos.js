$(function () {
    
    /*var dataFiles = [];
    $("#file1").change(function (e) {
        dataFiles = [];
        $.each(e.target.files, function (index, file) {
            console.log(file['FileUpload']);
            dataFiles.push({ "FileUpload": file });
        });
        console.log(dataFiles);
    })
    $('#btnUpload').click(function () {
        var fileUploadUrl = $('#UploadFileURL').data('request-url');
        var files = $("#file1");
        var totalFiles = files[0].files.length;
        var data = new FormData();
        for (var i = 0; i < totalFiles; i++){
            data.append("Foto", files[0].files[i]);
            console.log(files[0].files[i])
        }
        $.ajax({
            url: fileUploadUrl,
            data: data,
            type: 'POST',
            contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
            processData: false,
            success: function (response) {
                console.log(response);
                //$('#uploadMsg').text('Files have been uploaded successfully');
            },
            error: function (error) {
                console.log(error);
                //$('#uploadMsg').text('Error has occured. Upload is failed');
            }
        });
        //var file = document.getElementById("file1").files[0];
        //files.append('fileU', file1);
            
        /*var xhr = new XMLHttpRequest();
        
        xhr.upload.addEventListener("progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, false);
        xhr.open("POST", "/Requisicion/UploadFiles");
        xhr.send(files);

        
    }); */
});
