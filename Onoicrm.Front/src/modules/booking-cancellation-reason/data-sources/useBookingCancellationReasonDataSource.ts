import {ListDataSourceConfig, useListDataSource} from "~/modules/data-sources";
import {
  IBookingCancellationReasonDataSource
} from "~/modules/booking-cancellation-reason/interfaces/IBookingCancellationReasonDataSource";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import axios from "axios";
import {ref} from "vue";

export const useBookingCancellationReasonDataSource  = (config:ListDataSourceConfig = new ListDataSourceConfig({})):IBookingCancellationReasonDataSource => {
  const statistics = ref<any[]>([]);
  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      ...config,
      className:"BookingCancellationReason"
    })
  )
  const clinicStore = useClinicStore();

  const getStatistics = async () => {
    try {
      const {data} = await axios.get(`/api/v1/public/BookingCancellationReason/${clinicStore.clinic.value?.id}/statistics/`);
      dataSource.items.value = data;
      return data;
    } catch (e) {
      throw e;
    }
  }


  return {...dataSource, statistics, getStatistics}
}
