$(function () {
    var dataForMorrisGorivo = "";
    getDataGorivoForMorris("");
    


    function IsJsonString(str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }
    



    /** GORIVO  **/

    function getDataGorivoForMorris(izabranDatum) {

        $("#loadingDIV").show();

        $.ajax({
            url: "HomeVozniPark/GetGorivoChart",
            type: 'get',
            data: {
                dateForChart: izabranDatum
            },
            success: function (data) {
                //console.log("AAA: " + data);
                dataForMorrisGorivo = data;
                if (!IsJsonString(data))
                {
                    alert("Nemate dozvolu za dijagram o poreklu pošiljki.");
                    $("#loadingDIV").hide();
                    return;
                }
                drawLineChartGorivo(izabranDatum);
                
                $("#loadingDIV").hide();

            },

            error: function () {
                alert('Ajax error!');
            }
        });

        $.ajax({
            url: "HomeVozniPark/GetKmChart",
            type: 'get',
            data: {
                dateForChart: izabranDatum
            },
            success: function (data) {
                //console.log("AAA: " + data);
                dataForMorrisGorivo = data;
                if (!IsJsonString(data)) {
                    alert("Nemate dozvolu za dijagram o poreklu pošiljki.");
                    $("#loadingDIV").hide();
                    return;
                }
               
                drawLineChartKm(izabranDatum);
                $("#loadingDIV").hide();

            },

            error: function () {
                alert('Ajax error!');
            }
        });


    }


    function drawLineChartGorivo(izabranDatum) {
        
        $("#morris-line-gorivo-chart").empty();
        var ykeys = [];
        var lineColors = [];
        ykeys.push('SumGorivo');

        

        lineColors.push('#d9534f');

        Morris.Line({
            element: 'morris-line-gorivo-chart',
            data: $.parseJSON(dataForMorrisGorivo),
            xkey: ['PeriodDatumGorivo'],
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

    function drawLineChartKm(izabranDatum) {
        
        $("#morris-line-km-chart").empty();
        var ykeys = [];
        var lineColors = [];
        ykeys.push('SumKm');



        lineColors.push('#d9534f');

        Morris.Line({
            element: 'morris-line-km-chart',
            data: $.parseJSON(dataForMorrisGorivo),
            xkey: ['PeriodDatumKm'],
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

    
});