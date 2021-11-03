/**
 * Bootstrap Table English translation
 * Author: Zhixin Wen<wenzhixin2010@gmail.com>
 */
(function ($) {
    'use strict';

    $.fn.bootstrapTable.locales['sr-SR'] = {
        formatLoadingMessage: function () {
            return ' <img src="../images/ui-anim_basic_16x16.gif" /> Sacekajte...';
        },
        formatRecordsPerPage: function (pageNumber) {
            return pageNumber + ' po strani';
        },
        formatShowingRows: function (pageFrom, pageTo, totalRows) {
            return 'Prikazano ' + pageFrom + ' od ' + pageTo + ' ukupno ' + totalRows + ' redova';
        },
        formatSearch: function () {
            return 'TRazi';
        },
        formatNoMatches: function () {
            return 'Unesite podatke u pretragu';
        },
        formatPaginationSwitch: function () {
            return 'Hide/Show pagination';
        },
        formatRefresh: function () {
            return 'Osvezi';
        },
        formatToggle: function () {
            return 'Toggle';
        },
        formatColumns: function () {
            return 'Kolone';
        },
        formatAllRows: function () {
            return 'Sve';
        },
        formatExport: function () {
            return 'Export podataka';
        },
        formatClearFilters: function () {
            return 'Clear filters';
        }
    };

    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['sr-SR']);

})(jQuery);
