/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class BookingGridPassengerRow {

    constructor($row) {
        this.changedCallback = null;
        this.$row = $row;
        this.attach();
        this.refresh();
    }

    attach() {
        this.$type = this.$row.find('select[name="PassengerRole"]');
        this.$passenger = this.$row.find('input[name="PassengerName"]');
        this.$license = this.$row.find('input[name="PilotLicense"]');

        var that = this;
        this.$type.change(function (e) { that.onTypeChanged(); })
    }

    refresh() {
        var type = this.$type.val();
        this.$passenger.prop('disabled', !type);
        this.$license.prop('disabled', type != 'pilot');
    }

    setVisiblity(visible) {
        if (visible) {
            this.$row.show();
        } else {
            this.$row.hide();
        }
    }

    getData() {
        return {
            role: this.$type.val(),
            name: this.$passenger.val(),
            license: this.$license.val()
        }
    }

    isEmpty() {
        var data = this.getData();
        return !data.role && !data.name && !data.license;
    }

    onTypeChanged() {
        this.refresh();

        if (this.changedCallback) {
            this.changedCallback();
        }
    }
}


