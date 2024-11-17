import {IBookingListDataSource} from "../interfaces/IBookingListDataSource";
import axios from "axios";
import {ListDataSourceConfig, useListDataSource, useObjectDataSource} from "~/modules/data-sources";

export const useBookingListDataSource = (config: ListDataSourceConfig = new ListDataSourceConfig({})): IBookingListDataSource => {
    const dataSource = useListDataSource(new ListDataSourceConfig({
        ...config,
        className: "booking"
    }))

    const getSchedule = async () => {
        const clinicId = dataSource.filter.value.find(f => f.name=="clinicId")?.value;
        const dateTimeRange = dataSource.filter.value.find(f => f.name=="dateTimeRange")?.value;
        const doctorId = dataSource.filter.value.find(f => f.name=="doctorId")?.value;
        const armchairId = dataSource.filter.value.find(f => f.name=="armchairId")?.value;

        let url = `/api/v1/public/booking/schedule/${clinicId}/${dateTimeRange}/`;

        if(doctorId){
            url += `?doctorId=${doctorId}`;
            if(armchairId){
                url += `&armchairId=${armchairId}`;
            }
        }

        if(armchairId){
            url += `?armchairId=${armchairId}`;
        }

        const {data} = await axios.get(url);

        dataSource.items.value = data;
    }


    const notifyPatient = async (id: number, text: string) => {
        try {
            await axios.post(`/api/v1/public/Booking/${id}/notify/patient`, `"${text}"`, {
                headers: {
                    "Content-Type": "application/json"
                }
            })
        }catch (e){
            throw e
        }
    }
    const notifyDoctor = async (id: number, text: string) => {
        try {
            await axios.post(`/api/v1/public/Booking/${id}/notify/doctor`, `"${text}"`, {
                headers: {
                    "Content-Type": "application/json"
                }
            })
        }catch (e){
            throw e
        }
    }
    const updateReport = async (id: number,model:any) => {
        try {
            const {data} = await axios.put(`/api/v1/public/booking/${id}/report/`, model);
            return data
        } catch (e){
            throw e;
        }
    }

    return {
        ...dataSource,
        getSchedule,
        notifyPatient,
        notifyDoctor,
        updateReport
    }
}
