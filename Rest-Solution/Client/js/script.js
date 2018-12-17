$(function () {
    $.ajax({
        url: "http://localhost:56789/api/books",
        type: "GET",
        dataType: "json"
    })
        .done(function (result) {
            var box = $(".alert.alert-success");
            box.empty();

            //for (var i = 0; i < result.length; i++) {
            //    box.append($("<p>").text(result[i].Author + " " + result[i].Title));
            //    //box.append($("<p>").text(result[i].Title));
            //}

            var tableBooks = $("#Books tbody");
            var firstElement = tableBooks.find("tr").detach();
            for (var j = 0; j < result.length; j++) {
                var row = result[j];
                var col = firstElement.children();
                col.eq(0).text(row.Title);
                col.eq(1).text(row.Author);
                tableBooks.append(firstElement.clone());

            }
           
        })
        .fail(function (xhr, status, err) {
            console.log(xhr, "fail");
            console.log(status, "fail");
            console.log(err, "fail");
        })
        .always(function (xhr, status) {
            console.log(xhr, "always");
            console.log(status, "always");
        });

    $.ajax({
        url: "http://localhost:56789/api/readers",
        type: "GET",
        dataType: "json"
    })
        .done(function (result) {
            var box = $(".alert.alert-success");
            box.empty();

            var tableReaders = $("#Readers tbody");
            var firstElement = tableReaders.find("tr").detach();

            for (var j = 0; j < result.length; j++) {
                var row = result[j];
                var col = firstElement.children();
                col.eq(0).text(row.Name);
                col.eq(1).text(row.Age);
                tableReaders.append(firstElement.clone());

            }

        })
        .fail(function (xhr, status, err) {
            console.log(xhr, "fail");
            console.log(status, "fail");
            console.log(err, "fail");
        })
        .always(function (xhr, status) {
            console.log(xhr, "always");
            console.log(status, "always");
        });


    var formRows = $("input");
    
    var button = $(".btn btn-success btn-sm");
    
    button.on("click", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var title = formRows[0];
        var author = formRows[1];

        $.ajax({
            url: "http://localhost:56789/api/books/",
            data: { Title: title, Author: author },
            type: "POST",
            dataType: "json"
        })
            .done(function (result) {

                var tableBooks = $("#Books tbody");
                var firstElement = tableBooks.find("tr").detach();
                for (var j = 0; j < result.length; j++) {
                    var row = result[j];
                    var col = firstElement.children();
                    col.eq(0).text(row.Title);
                    col.eq(1).text(row.Author);
                    tableBooks.append(firstElement.clone());

                }
                var box = $(".alert.alert-success").html($("<div>")
                    .text(result.Title).addClass(result.Author));

            });

    });
    //var edit = $(".btn btn-primary btn-sm");
    //var delete = $(".btn btn-danger btn-sm");
    //var borrow = $(".btn btn-info btn-sm");

    //button.on("click", function (e) {
    //    e.preventDefault();
    //    var id = $(this).data("id");
    //    var title = formRows[0];
    //    var author = formRows[1];
    //    //console.log(id, "id");
    //    $.ajax({
    //            url: "http://localhost:56789/api/books/" + id,
    //            data: { Title: title, Author: author },
    //            type: "POST",
    //            dataType: "json"
    //        })
    //        .done(function (result) {
    //            //console.log(result, "result dla id 3");
    //            var box = $(".alert.alert-success").html($("<div>")
    //                .text(result.Title).addClass(result.Author));

    //        });

    //});
});