$(function () {
    var dataForMorrisObrisane = "";

    

    $('#datum-area-izbrisane').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-area-izbrisane').val();

            if (izabranDatum !== "") {
                getDataObrisaneForMorris(izabranDatum, 0, "");
                drawDonutChartIzbrisanePoreklo(izabranDatum);
            }

            $('#lbl_naslov_line, #lbl_naslov_bar, #lbl_naslov_bar_stacked, #lbl_naslov_donut').text("Izbrisane posiljke " + izabranDatum); 
            $('#datum-area-izbrisane').data('dateRangePicker').clear();
        });

    $('#datum-bar-izbrisane').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-bar-izbrisane').val();

            if (izabranDatum !== "") {
                getDataObrisaneForMorris(izabranDatum, 3, "");
            }

            $('#lbl_naslov_bar').text("Izbrisane posiljke " + izabranDatum);
            $('#datum-bar-izbrisane').data('dateRangePicker').clear();
        });

    $('#datum-bar-stacked-izbrisane').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-bar-stacked-izbrisane').val();

            if (izabranDatum !== "") {
                getDataObrisaneForMorris(izabranDatum, 4, "");
            }

            $('#lbl_naslov_bar_stacked').text("Izbrisane posiljke " + izabranDatum);
            $('#datum-bar-stacked-izbrisane').data('dateRangePicker').clear();
        });

    $('#datum-donut-izbrisane').dateRangePicker({ endDate: moment().endOf('day').format('YYYY-MM-DD') })
        .bind('datepicker-closed', function () {
            var izabranDatum = $('#datum-donut-izbrisane').val();

            if (izabranDatum !== "") {
                drawDonutChartIzbrisanePoreklo(izabranDatum);
            }

            $('#lbl_naslov_donut').text("Izbrisane posiljke " + izabranDatum);
            $('#datum-donut-izbrisane').data('dateRangePicker').clear();
        });

    

    $('#izvIzbrisaneLine').click(function () {
        $('#card-donut').show();
        $('#datum-area-evidentirane, #datum-bar-evidentirane, #datum-bar-stacked-evidentirane, #datum-area-evidentirane-poreklo, #datum-bar-evidentirane-poreklo, #datum-bar-stacked-evidentirane-poreklo, #datum-donut-evidentirane-poreklo').hide();
        $('#datum-area-izbrisane,  #datum-bar-izbrisane,  #datum-bar-stacked-izbrisane, #datum-donut-izbrisane').show();

        $('#lbl_naslov_donut, #lbl_naslov_area, #lbl_naslov_line, #lbl_naslov_area, #lbl_naslov_bar, #lbl_naslov_bar_stacked').text($('#izvIzbrisaneLine').text());
        $('#morris-area-izbrisane-chart, #morris-line-izbrisane-chart, #morris-bar-izbrisane-chart, #morris-bar-stacked-izbrisane-chart, #morris-donut-izbrisane-chart  ').show();
        $('#morris-area-evidentirane-chart, #morris-line-evidentirane-chart, #morris-bar-evidentirane-chart, #morris-bar-stacked-evidentirane-chart, #morris-donut-evidentirane-chart ').hide();
        $('#morris-area-evidentirane-poreklo-chart, #morris-line-evidentirane-poreklo-chart, #morris-bar-evidentirane-poreklo-chart, #morris-bar-stacked-evidentirane-poreklo-chart, #morris-donut-evidentirane-poreklo-chart ').hide();

        getDataObrisaneForMorris("", 0, "");
        drawDonutChartIzbrisanePoreklo("");

        //$('#datum-area-izbrisane').data('dateRangePicker').clear();

    });

   

    $('#izvIzbrisaneBar').click(function () {

        
        $('#datum-bar-evidentirane, #datum-bar-evidentirane-poreklo').hide();
        $('#datum-bar-izbrisane').show();


        $('#lbl_naslov_bar').text($('#izvIzbrisaneBar').text());
        $('#morris-bar-izbrisane-chart').show();
        $('#morris-bar-evidentirane-poreklo-chart').hide();
        $('#morris-bar-evidentirane-chart').hide();

        if (dataForMorrisObrisane === "") {
            getDataObrisaneForMorris("", 3, "");
        } else {
            drawBarChartObrisanePoreklo("", "");
        }

        //$('#datum-area-izbrisane').data('dateRangePicker').clear();

    });

    $('#izvIzbrisaneBarStacked').click(function () {

       
        $('#datum-bar-stacked-evidentirane, #datum-bar-stacked-evidentirane-poreklo').hide();
        $('#datum-bar-stacked-izbrisane').show();

        $('#lbl_naslov_bar_stacked').text($('#izvIzbrisaneBarStacked').text());
        $('#morris-bar-stacked-izbrisane-chart').show();
        $('#morris-bar-stacked-evidentirane-poreklo-chart').hide();
        $('#morris-bar-stacked-evidentirane-chart').hide();

        if (dataForMorrisObrisane === "") {
            getDataObrisaneForMorris("", 4, "");
        } else {
            drawBarStackedChartObrisanePoreklo("", "");
        }

        //$('#datum-area-izbrisane').data('dateRangePicker').clear();

    });

    $('#izvIzbrisaneDonut').click(function () {

        $('#lbl_naslov_donut').text($('#izvIzbrisaneDonut').text());
        $('#morris-donut-izbrisane-chart').show();
        $('#morris-donut-evidentirane-poreklo-chart').hide();

       
        $('#datum-donut-evidentirane-poreklo').hide();
        $('#datum-donut-izbrisane').show();

        drawDonutChartObrisanePoreklo("", "");

    });

    /************************************************************ IZBRISANE POREKLO *********************************************/
    function IsJsonString(str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }

    function getDataObrisaneForMorris(izabranDatum, tipMorris, poreklo) {
        /* tipMorris je tip dijagrama koji se ucitava
           0 - ucitaj sve
           1 - Area
           2 - Line
           3 - Bar
           4 - BarStacked
         */

        $("#loadingDIV").show();

        $.ajax({
            url: "Home/GetObrisanePorekloChart",
            type: 'get',
            data: {
                dateForChart: izabranDatum
            },
            success: function (data) {
                console.log("AAA: " + data);
                if (!IsJsonString(data))
                {
                    alert("Nemate dozvolu za dijagram o poreklu izbrisanih pošiljki.");
                    $("#loadingDIV").hide();
                    return;
                }
                dataForMorrisObrisane = data;

                if (tipMorris === 0) {
                    //drawAreaChartObrisanePoreklo(izabranDatum, poreklo);
                    drawLineChartObrisanePoreklo(izabranDatum, poreklo);
                    drawBarChartObrisanePoreklo(izabranDatum, poreklo);
                    drawBarStackedChartObrisanePoreklo(izabranDatum, poreklo);
                } 
                else if (tipMorris === 2)
                    drawLineChartObrisanePoreklo(izabranDatum, poreklo);
                else if (tipMorris === 3)
                    drawBarChartObrisanePoreklo(izabranDatum, poreklo);
                else if (tipMorris === 4)
                    drawBarStackedChartObrisanePoreklo(izabranDatum, poreklo);

                //else if (tipMorris === 1)
                //    drawAreaChartObrisanePoreklo(izabranDatum, poreklo);

                $("#loadingDIV").hide();

            },

            error: function () {
                alert('Ajax error!');
            }
        });

        
    }

    function drawDonutChartIzbrisanePoreklo(izabranDatum) {
        $("#morris-donut-izbrisane-chart").empty();
        $.ajax({
            url: "Home/GetObrisanePorekloDonutChart",
            type: 'get',
            data: {
                dateForChart: izabranDatum
            },
            success: function (data) {
                console.log(data);
                
                Morris.Donut({
                    element: 'morris-donut-izbrisane-chart',
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

    function drawAreaChartObrisanePoreklo(izabranDatum, poreklo) {
        $("#morris-area-izbrisane-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('klijentski');
            ykeys.push('shipping');
            ykeys.push('magacioner');
            ykeys.push('test');
            ykeys.push('duplirana');
            ykeys.push('posiljalac');
            ykeys.push('narucilac');
            ykeys.push('gabarit');


            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "klijentski") lineColors.push('#d9534f');
            else if (poreklo === "shipping") lineColors.push('black');
            else if (poreklo === "magacioner") lineColors.push('gray');
            else lineColors.push('orange');
        }

        Morris.Area({
            element: 'morris-area-izbrisane-chart',
            data: $.parseJSON(dataForMorrisObrisane),
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

    function drawLineChartObrisanePoreklo(izabranDatum, poreklo) {
        $("#morris-line-izbrisane-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('klijentski');
            ykeys.push('shipping');
            ykeys.push('magacioner');
            ykeys.push('test');
            ykeys.push('duplirana');
            ykeys.push('posiljalac');
            ykeys.push('narucilac');
            ykeys.push('gabarit');


            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "klijentski") lineColors.push('#d9534f');
            else if (poreklo === "shipping") lineColors.push('black');
            else if (poreklo === "magacioner") lineColors.push('gray');
            else lineColors.push('orange');
        }

        Morris.Line({
            element: 'morris-line-izbrisane-chart',
            data: $.parseJSON(dataForMorrisObrisane),
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

    function drawBarChartObrisanePoreklo(izabranDatum, poreklo) {
        $("#morris-bar-izbrisane-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('klijentski');
            ykeys.push('shipping');
            ykeys.push('magacioner');
            ykeys.push('test');
            ykeys.push('duplirana');
            ykeys.push('posiljalac');
            ykeys.push('narucilac');
            ykeys.push('gabarit');


            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "klijentski") lineColors.push('#d9534f');
            else if (poreklo === "shipping") lineColors.push('black');
            else if (poreklo === "magacioner") lineColors.push('gray');
            else lineColors.push('orange');
        }

        Morris.Bar({
            element: 'morris-bar-izbrisane-chart',
            data: $.parseJSON(dataForMorrisObrisane),
            xkey: 'period',
            ykeys: ykeys,
            labels: ykeys,
            hideHover: 'auto',
            resize: true,
            barColors: lineColors
        });
    }

    function drawBarStackedChartObrisanePoreklo(izabranDatum, poreklo) {
        $("#morris-bar-stacked-izbrisane-chart").empty();
        var ykeys = [];
        var lineColors = [];
        if (poreklo === "") {
            ykeys.push('klijentski');
            ykeys.push('shipping');
            ykeys.push('magacioner');
            ykeys.push('test');
            ykeys.push('duplirana');
            ykeys.push('posiljalac');
            ykeys.push('narucilac');
            ykeys.push('gabarit');


            lineColors.push('#d9534f', 'black', 'gray', 'orange');

        } else {
            ykeys.push(poreklo);
            if (poreklo === "klijentski") lineColors.push('#d9534f');
            else if (poreklo === "shipping") lineColors.push('black');
            else if (poreklo === "magacioner") lineColors.push('gray');
            else lineColors.push('orange');
        }

        Morris.Bar({
            element: 'morris-bar-stacked-izbrisane-chart',
            data: $.parseJSON(dataForMorrisObrisane),
            xkey: 'period',
            ykeys: ykeys,
            labels: ykeys,
            stacked: true,
            hideHover: 'auto',
            resize: true,
            barColors: lineColors
        });
    }
});