﻿$(function () {

    var noteids = [];

    $("div[data-note-id]").each(function (i, e) {
        noteids.push($(e).data("note-id"));
    });
    console.log(noteids);
    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }
    }).done(function (data) {
        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.find("span.like-star");


                btn.data("liked", true);
                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }
        }
    }).fail(function () {
    });


    $("button[data-liked]").click(function () {

        var btn = $(this);
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");

        var spanStar = btn.find("span.like-star");
        var spancount = btn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked }
        }).done(function (data) {

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                liked = !liked;
                btn.data("liked", liked);
                spancount.text(data.result);

                spanStar.removeClass("glyphicon-star-empty");
                spanStar.removeClass("glyphicon-star");

                if (liked) {
                    spanStar.addClass("glyphicon-star");
                } else {
                    spanStar.addClass("glyphicon-star-empty");
                }
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.")
        });
    });

});
