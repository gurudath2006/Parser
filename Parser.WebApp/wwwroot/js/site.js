var loadData;
var btn_load_url_selector = '#btn__load-url';
var input_url_selector = '#inputUrl';
var warning_selector = '#input-warning';
var label_word_count_selector = '#total_word_count';
var container_carosel_selector = '#carousel_container';
var container_load_results_selector = '#results';
var hdn_api_end_point_selector = '#api_end_point';

$(document).ready(function () {    

    $(btn_load_url_selector).click(function () {
        var url = $(input_url_selector).val();
        // Validate input
        if (url == '') {
            $(warning_selector).removeClass('parser-field_hidden');
            $(container_load_results_selector).addClass('parser-field_hidden');
            return;
        }
        else {
            $(warning_selector).addClass('parser-field_hidden');
            $(container_load_results_selector).removeClass('parser-field_hidden');
        }

        loadContent(url);
    });
});

function loadContent(url) {
    var apiEndPoint = $(hdn_api_end_point_selector).val();
    $.getJSON(apiEndPoint + url, function (data) {
        if (data.isSuccess == true) {
            // Show results
            $(container_load_results_selector).removeClass('parser-field_hidden');

            loadData = data;

            // Bind Carousel
            $(container_carosel_selector).html('<div id="carousel" style="width:600px;height:400px;padding:5px;background-color:gray"></div>');
            $(data.images).each(function (index) {
                $('#carousel').append('<img src="' + this + '" />');
            });

            $('#carousel').slick({
                dots: true,
                infinite: true,
                speed: 500,
                fade: true,
                cssEase: 'linear',
                autoplay: true,
                autoplaySpeed: 500,
            });

            // Bind word count

            $(label_word_count_selector).html(data.totalCount)

            // Bind Chart

            google.charts.load('current', { packages: ['corechart', 'bar'] });
            google.charts.setOnLoadCallback(function () {
                drawSeries();
            });
        }
    });
}

function drawSeries() {
    var dataItems = [];
    dataItems.push(['Word', 'Count']);
    $(loadData.words).each(function (index) {
        dataItems.push([this.item1, this.item2])
    });

    var data = google.visualization.arrayToDataTable(dataItems);

    var options = {
        title: 'Top 10 words',
        chartArea: { width: '50%' },
        hAxis: {
            title: 'Count',
            minValue: 0
        },
        vAxis: {
            title: 'Word'
        },
        width: 400,
        height: 300
    };

    var chart = new google.visualization.BarChart(document.getElementById('chart'));
    chart.draw(data, options);
}
