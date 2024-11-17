import {Clinic} from "../Clinic";
import {ref} from "vue";
import {ObjectDataSourceConfig, UpdateFieldModel, useObjectDataSource} from "~/modules/data-sources";
import axios from "axios";
import {defineStore} from "pinia";

export const useClinicStore = defineStore("clinic",() => {
    const clinic = ref<Clinic|null>(null);
    const dataSource = useObjectDataSource(
        new ObjectDataSourceConfig({
            className:"clinic",
        })
    )

    const getClinic = async (clinicId:number) => {
        const {data} = await axios.get(`/api/v1/public/clinic/${clinicId}/`);
        clinic.value = data;
    }


    const switchBookingType = async () => {
        dataSource.model.value = clinic.value;
        dataSource.config.value.id = clinic.value?.id as number;
        clinic.value = await dataSource.updateField(clinic.value?.id ?? 0, [
            new UpdateFieldModel({
                fieldName: "bookingType",
                fieldValue: clinic.value?.bookingType == 0 ? 1 : 0
            })
        ])
    }


    return {clinic, switchBookingType, getClinic, ...dataSource}
})
