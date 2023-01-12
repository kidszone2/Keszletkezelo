/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class BookingGridForm {
    
    constructor (selector) {
        this.$grid = $(selector);
        this.attach();
        this.refresh();
    }

    attach() {
        var that = this;

        that.rows = [];        
        this.$grid.find('[data-role="tf-passenger-row"]')
            .each(function(idx, element) {
                var row = new BookingGridPassengerRow($(element));
                row.changedCallback = function() { that.onRowChanged(); };
                that.rows.push(row);
            });
    }

    refresh() {
        var isVisible = true;
        this.rows.forEach(function(row) {
            row.setVisiblity(isVisible);
            isVisible = !row.isEmpty();
        });
    }

    onRowChanged() {
        this.refresh();

    }
}