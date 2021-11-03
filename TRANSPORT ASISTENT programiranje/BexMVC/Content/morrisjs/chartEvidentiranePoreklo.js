$(function () {
    var dataForMorrisEvidentiranePoreklo = "";

    $('#datum-area-evidentirane-poreklo').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-area-evidentirane-poreklo').val();

            if (izabranDatum !== "") {
                getDataEvidentiranePorekloForMorris(izabranDatum, 0, "");
                drawDonutChartEvidentiranePoreklo(izabranDatum);
            }

            $('#lbl_naslov_line, #lbl_naslov_bar, #lbl_naslov_bar_stacked, #lbl_naslov_donut').text("Evidentirane posiljke - poreklo " + izabranDatum); 
            $('#datum-area-evidentirane-poreklo').data('dateRangePicker').clear();
        });

    $('#datum-bar-evidentirane-poreklo').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-bar-evidentirane-poreklo').val();

            if (izabranDatum !== "") {
                getDataEvidentiranePorekloForMorris("", 3, "");
            }

            $('#lbl_naslov_bar').text("Evidentirane posiljke - poreklo " + izabranDatum); 
            $('#datum-bar-evidentirane-poreklo').data('dateRangePicker').clear();
        });

    $('#datum-bar-stacked-evidentirane-poreklo').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-bar-stacked-evidentirane-poreklo').val();

            if (izabranDatum !== "") {
                getDataEvidentiranePorekloForMorris("", 4, "");
            }

            $('#lbl_naslov_bar_stacked').text("Evidentirane posiljke - poreklo " + izabranDatum);
            $('#datum-bar-stacked-evidentirane-poreklo').data('dateRangePicker').clear();
        });

    $('#datum-donut-evidentirane-poreklo').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-donut-evidentirane-poreklo').val();

            if (izabranDatum !== "") {
                drawDonutChartEvidentiranePoreklo(izabranDatum);
            }

            $('#lbl_naslov_donut').text("Evidentirane posiljke - poreklo " + izabranDatum);
            $('#datum-donut-evidentirane-poreklo').data('dateRangePicker').clear();
        });


    $('#izvEvidentiranihOdakleLine').click(function () {

        $('#card-donut').show();
        $('#datum-area-evidentirane, #datum-bar-evidentirane, #datum-bar-stacked-evidentirane, #datum-area-izbrisane,  #datum-bar-izbrisane,  #datum-bar-stacked-izbrisane, #datum-donut-izbrisane').hide();
        $('#datum-area-evidentirane-poreklo, #datum-bar-evidentirane-poreklo, #datum-bar-stacked-evidentirane-poreklo, #datum-donut-evidentirane-poreklo ').show();


        $('#lbl_naslov_donut, #lbl_naslov_area, #lbl_naslov_line, #lbl_naslov_area, #lbl_naslov_bar, #lbl_naslov_bar_stacked').text($('#izvEvidentiranihOdakleLine').text());
        $('#morris-area-izbrisane-chart, #morris-line-izbrisane-chart, #morris-bar-izbrisane-chart, #morris-bar-stacked-izbrisane-chart, #morris-donut-izbrisane-chart  ').hide();
        $('#morris-area-evidentirane-poreklo-chart, #morris-line-evidentirane-poreklo-chart, #morris-bar-evidentirane-poreklo-chart, #morris-bar-stacked-evidentirane-poreklo-chart, #morris-donut-evidentirane-poreklo-chart ').show();
        $('#morris-area-evidentirane-chart, #morris-line-evidentirane-chart, #morris-bar-evidentirane-chart, #morris-bar-stacked-evidentirane-chart, #morris-donut-evidentirane-chart ').hide();

        getDataEvidentiranePorekloForMorris("", 0, "");
        drawDonutChartEvidentiranePoreklo("");
        

        //$('#datum-area-evidentirane-poreklo').data('dateRangePicker').clear();

    });


    $('#izvEvidentiranihOdakleBar').click(function () {

        $('#lbl_naslov_bar').text($('#izvEvidentiranihOdakleBar').text());
        $('#morris-bar-izbrisane-chart').hide();
        $('#morris-bar-evidentirane-poreklo-chart').show();
        $('#morris-bar-evidentirane-chart').hide();

        if (dataForMorrisEvidentiranePoreklo === "") {
            getDataEvidentiranePorekloForMorris("", 3, "");
        } else {
            drawBarChartEvidentiranePoreklo("", "");
        }

        $('#datum-area-evidentirane-poreklo').data('dateRangePicker').clear();

    });

    $('#izvEvidentiranihOdakleBarStacked').click(function () {

        $('#lbl_naslov_bar_stacked').text($('#izvEvidentiranihOdakleBarStacked').text());
        $('#morris-bar-stacked-izbrisane-chart').hide();
        $('#morris-bar-stacked-evidentirane-poreklo-chart').show();
        $('#morris-bar-stacked-evidentirane-chart').hide();

        if (dataForMorrisEvidentiranePoreklo === "") {
            getDataEvidentiranePorekloForMorris("", 4, "");
        } else {
            drawBarStackedChartEvidentiranePoreklo("", "");
        }

        $('#datum-area-evidentirane-poreklo').data('dateRangePicker').clear();

    });

    $('#izvEvidentiranihOdakleDonut').click(function () {

        $('#lbl_naslov_donut').text($('#izvEvidentiranihOdakleDonut').text());

        $('#morris-donut-izbrisane-chart').hide();
        $('#morris-donut-evidentirane-poreklo-chart').show();

        drawDonutChartEvidentiranePoreklo("", "");

    });


    function IsJsonString(str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }
    



    /** EVIDENTIRANE POREKLO **/

    function getDataEvidentiranePorekloForMorris(izabranDatum, tipMorris, poreklo) {
        /* tipMorris je tip dijagrama koji se ucitava
           0 - ucitaj sve
           1 - Area
           2 - Line
           3 - Bar
           4 - BarStacked
         */

        $("#loadingDIV").show();

        $.ajax({
            url: "Home/GetEvidentiranePorekloChart",
            type: 'get',
            data: {
                dateForChart: izabranDatum
            },
            success: function (data) {
                //console.log("AAA: " + data);
                dataForMorrisEvidentiranePoreklo = data;
                if (!IsJsonString(data))
                {
                    alert("Nemate dozvolu za dijagram o poreklu pošiljki.");
                    $("#loadingDIV").hide();
                    return;
                }
                if (tipMorris === 0) {
                   // drawAreaChartEvidentiranePoreklo(izabranDatum, poreklo);
                    drawLineChartEvidentiranePoreklo(izabranDatum, poreklo);
                    drawBarChartEvidentiranePoreklo(izabranDatum, poreklo);
                    drawBarStackedChartEvidentiranePoreklo(izabranDatum, poreklo);
                } 
                else if (tipMorris === 2)
                    drawLineChartEvidentiranePoreklo(izabranDatum, poreklo);
                else if (tipMorris === 3)
                    drawBarChartEvidentiranePoreklo(izabranDatum, poreklo);
                else if (tipMorris === 4)
                    drawBarStackedChartEvidentiranePoreklo(izabranDatum, poreklo);

                //else if (tipMorris === 1)
                //    drawAreaChartEvidentiranePoreklo(izabranDatum, poreklo);
                $("#loadingDIV").hide();

            },

            error: function () {
                alert('Ajax error!');
            }
        });


    }


    function drawAreaChartEvidentiranePoreklo(izabranDatum, poreklo) {
        $("#morris-area-evidentirane-poreklo-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('callcenter');
            ykeys.push('klijentski');
            ykeys.push('integracija');
            ykeys.push('web');

            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "callcenter") lineColors.push('#d9534f');
            if (poreklo === "klijentski") lineColors.push('black');
            if (poreklo === "integracija") lineColors.push('gray');
            if (poreklo === "web") lineColors.push('orange');
        }

        Morris.Area({
            element: 'morris-area-evidentirane-poreklo-chart',
            data: $.parseJSON(dataForMorrisEvidentiranePoreklo),
            xkey: ['period'],
            ykeys: ykeys,
            //ymin: $('#prijavljenoPosiljkePetDana').val(),
            //ymax:10000,
            labels: ykeys,
            pointSize: 3,
            hideHover: 'auto',
            resize: true,
            parseTime: false,
            fillOpacity: 0.6,
            behaveLikeLine: true,
            pointFillColors: ['#ffffff'],
            pointStrokeColors: ['black'],
            lineColors: lineColors
        });
    }



    function drawLineChartEvidentiranePoreklo(izabranDatum, poreklo) {
        $("#morris-line-evidentirane-poreklo-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('callcenter');
            ykeys.push('klijentski');
            ykeys.push('integracija');
            ykeys.push('web');

            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "callcenter") lineColors.push('#d9534f');
            if (poreklo === "klijentski") lineColors.push('black');
            if (poreklo === "integracija") lineColors.push('gray');
            if (poreklo === "web") lineColors.push('orange');
        }

        Morris.Line({
            element: 'morris-line-evidentirane-poreklo-chart',
            data: $.parseJSON(dataForMorrisEvidentiranePoreklo),
            xkey: ['period'],
            ykeys: ykeys,
            labels: ykeys,
            pointSize: 3,
            hideHover: 'auto',
            resize: true,
            parseTime: false,
            fillOpacity: 0.6,
            behaveLikeLine: true,
            pointFillColors: ['#ffffff'],
            pointStrokeColors: ['black'],
            lineColors: lineColors
        });
    }

    function drawBarChartEvidentiranePoreklo(izabranDatum, poreklo) {
        $("#morris-bar-evidentirane-poreklo-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('callcenter');
            ykeys.push('klijentski');
            ykeys.push('integracija');
            ykeys.push('web');

            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "callcenter") lineColors.push('#d9534f');
            if (poreklo === "klijentski") lineColors.push('black');
            if (poreklo === "integracija") lineColors.push('gray');
            if (poreklo === "web") lineColors.push('orange');
        }

        Morris.Bar({
            element: 'morris-bar-evidentirane-poreklo-chart',
            data: $.parseJSON(dataForMorrisEvidentiranePoreklo),
            xkey: 'period',
            ykeys: ykeys,
            labels: ykeys,
            hideHover: 'auto',
            resize: true,
            barColors: lineColors
        });
    }

    function drawBarStackedChartEvidentiranePoreklo(izabranDatum, poreklo) {
        $("#morris-bar-stacked-evidentirane-poreklo-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('callcenter');
            ykeys.push('klijentski');
            ykeys.push('integracija');
            ykeys.push('web');

            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "callcenter") lineColors.push('#d9534f');
            if (poreklo === "klijentski") lineColors.push('black');
            if (poreklo === "integracija") lineColors.push('gray');
            if (poreklo === "web") lineColors.push('orange');
        }
        Morris.Bar({
            element: 'morris-bar-stacked-evidentirane-poreklo-chart',
            data: $.parseJSON(dataForMorrisEvidentiranePoreklo),
            xkey: 'period',
            ykeys: ykeys,
            labels: ykeys,
            stacked: true,
            hideHover: 'auto',
            resize: true,
            barColors: lineColors
        });
    }

    function drawDonutChartEvidentiranePoreklo(izabranDatum) {
        $("#morris-donut-evidentirane-poreklo-chart").empty();
        $.ajax({
            url: "Home/GetEvidentiranePorekloDonutChart",
            type: 'get',
            data: {
                dateForChart: izabranDatum
            },
            success: function (data) {
                console.log(data);

                Morris.Donut({
                    element: 'morris-donut-evidentirane-poreklo-chart',
                    data: $.parseJSON(data),
                    resize: true,
                    colors: ['#d9534f', 'black', 'gray', 'silver']
                });
            },
            error: function () {
                alert('Ajax error!');
            }
        });
    }
});