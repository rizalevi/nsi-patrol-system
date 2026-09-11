let loadingCounter = 0;

function showGlobalLoading(message = "Please wait...") {

    loadingCounter++;

    $("#globalLoadingText").text(message);

    $("#globalLoading").css("display", "flex");
}

function hideGlobalLoading() {

    loadingCounter--;

    if (loadingCounter <= 0) {

        loadingCounter = 0;

        $("#globalLoading").hide();
    }
}