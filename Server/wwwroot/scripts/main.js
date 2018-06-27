// Should be packed

let getTvMazeShowsApiUrl = "api/v1/tvmaze/shows?query=";
let importTvMazeShowApiUrl = "api/v1/tvmaze/shows/import";

function loadTvMazeShows() {
    let apiUrl = getTvMazeShowsApiUrl + $('#search-input').val();

    $.ajax({
        url: apiUrl,
        type: "GET",
        contentType: "application/json",
        success: function (shows) {
            DrawTvMazeTable(shows);
        },
        error: function (http) {
            alert("Error: Status code: " + http.status + " Message: " + http.responseText);
        }
    });
};

function DrawTvMazeTable(shows) {
    var table = '<table class="showsTable">';

    table += '<tr>';
    table += '<th> Id </th>';
    table += '<th> Name </th>';
    table += '<th> Actions </th>';
    table += '</tr>';

    for (var i = 0; i < shows.length; i++) {
        table += '<tr>';
        table += '<td>' + shows[i].id + '</td>';
        table += '<td>' + shows[i].name + '</td>';
        table += '<td> <button onclick="importTvMazeShow(' + shows[i].id + ');">Import</button></td>';
        table += '</tr>';
    }

    table += '</table>';

    $('#tvmaze-table-container').html(table);
}

function importTvMazeShow(id) {
    let apiUrl = importTvMazeShowApiUrl;

    $.ajax({
        url: apiUrl,
        type: "POST",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (http) {
            alert("Done: show=" + id + " successfully imported!");
        },
        error: function (http) {
            alert("Error: show=" + id + " not imported! Status code: " + http.status);
        }
    });
}