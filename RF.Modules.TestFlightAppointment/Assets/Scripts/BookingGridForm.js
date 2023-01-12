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
    }

    attach() {
        var that = this;

        that.rows = [];        
        this.$grid.find('[data-role="tf-passenger-row"]')
            .each(function(idx, element) {
                var row = new BookingGridPassengerRow($(element));
                that.rows.push(row);
            });
    }
}