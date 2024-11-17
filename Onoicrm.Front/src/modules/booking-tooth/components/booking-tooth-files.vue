<template>
  <div v-if="model.files.length>0" class=" p-3 card--memo border-round-xl ">
    <div class="grid">
        <file-list
            :items="model.files"
            @remove="remove($event)"
            v-slot="{item}"
            css-class="col-6 relative doctor-files"
        >
          <div class="hover-remove">
            <img style="object-fit:cover" :src="getFileUrl(item)" class="w-full h-5rem border-round-xl" alt="">
            <i @click="remove(item)" class="pi pi-trash text-red-500 absolute top-50 left-50" style="transform: translate(-50%,-50%)"></i>
          </div>
        </file-list>

      <div class="col-6">
        <file-selector v-slot="{dragEnter}" @select="upload($event)" class="w-full h-5rem">
        <div class="text-3xl w-full h-full p-button border-round-xl  border-none bg-gray-500 flex align-items-center justify-content-center">
          +
        </div>
      </file-selector>
      </div>
    </div>
  </div>
  <div v-else>
    <file-selector v-slot="{dragEnter}" @select="upload($event)" class="border-round-2xl cursor-pointer overflow-auto min-h-17rem h-14rem md:h-full w-full p-3 card--memo ">
      <div class="w-full flex justify-content-center align-items-center h-full">
        <div style="border: 1px dashed #696969;"  class=" w-9 border-round-xl doctor-dialog-title--gray font-medium p-3 flex align-items-center justify-content-center cursor-pointer">
          <i class="pi pi-image text-2xl mr-2 cursor-pointer"></i>
          <p class="w-8 cursor-pointer">
            переташите файл сюда
          </p>
        </div>
      </div>
    </file-selector>
  </div>

</template>

<script setup lang="ts">
import {defaultConfirm, defaultMessage, FileSelector, IMessage, TConfirm} from "~/shared"
import {IListDataSource} from "~/modules/data-sources";
import {inject, ref} from "vue";
import FileList from "~/modules/files/components/file-list.vue";
import {BookingToothFile} from "~/modules/booking-tooth/entities/BookingToothFile";
import {BookingTooth} from "~/modules/tooth";
const {dataSource,model} = defineProps<{
  dataSource:IListDataSource<any>,
  model:BookingTooth
}>();
const displayTrashIcon = ref(false)
const confirm = inject<TConfirm>("confirm",defaultConfirm);
const message = inject<IMessage>("message",defaultMessage)

const upload = async (file:File) => {
  const base64 = await getBase64FromFile(file);
  model.files.push(new BookingToothFile({
    name:file.name,
    fileSize:file.size,
    extension:getFileExtension(file.name),
    base64
  }))
}

const getFileUrl = (file:BookingToothFile) => `data:image/jpeg;base64,${file.base64}`;

const getFileExtension = (fileName: string): string => {
  const lastDotIndex = fileName.lastIndexOf('.');
  return lastDotIndex !== -1 ? fileName.substring(lastDotIndex + 1) : '';
};

const getBase64FromFile = (file: File): Promise<string | null> => new Promise((resolve, reject) => {
  const reader = new FileReader();
  reader.onload = () => {
    const base64 = reader.result?.toString().split(',')[1] || null;
    resolve(base64);
  };
  reader.onerror = (error) => {
    reject(error);
  };
  reader.readAsDataURL(file);
});

const remove = (file:any) => {
  model.files = model.files.filter(f => f.name !== file.name)
  message.success("Файл успешно удалён")
}


</script>



