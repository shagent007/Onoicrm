<template>
  <uploader
    v-for="file in files"
    :key="file.name"
    :file="file"
    :data-source="dataSource"
    @finish="finish($event)"
  />
  <div class="h-15rem overflow-auto">
    <file-list
      :items="fileDataSource.items.value"
      @remove="remove($event)"
      v-slot="{item}"
      css-class="flex align-items-center justify-content-between mb-3 pr-3"
    >
      <div class="flex align-items-center">
        <div class="w-7rem h-5rem mr-2 border-round-sm">
          <img
              class="w-full h-full border-round-sm"
              style="object-fit:cover; object-position:center;"
              :src="`/api/v1/file/${item.id}`"
              :alt="item.alt"
          />
        </div>
        <div>
          <h4 class="text-xs md:text-base">{{ item.name }}</h4>
          <p class="text-600 text-xs md:text-base">{{item.size}}</p>
        </div>
      </div>
      <div class="flex align-items-center">
        <i
            class="pi pi-pencil cursor-pointer text-primary pr-3"
            style="font-size: 16px"
        />
        <i
            class="pi pi-trash cursor-pointer"
            style="color: red;
            font-size: 18px"
            @click="remove(item)"
        />
      </div>
    </file-list>
  </div>
  <file-selector v-slot="{dragEnter}" @select="upload($event)">
    <div class="mt-4 w-full p-button bg-blue-300 border-none surface-900">
      + Добавить еще фото или перетащите сюда...
    </div>
  </file-selector>
</template>

<script setup lang="ts">
import Uploader from "~/modules/files/components/file-uploader.vue";
import {defaultConfirm, defaultMessage, FileSelector, IMessage, TConfirm} from "~/shared"
import {IBookingDataSource, ListDataSourceConfig, useListDataSource} from "~/modules/data-sources";
import {inject, ref} from "vue";
import FileList from "~/modules/files/components/file-list.vue";
const {dataSource} = defineProps<{dataSource:IBookingDataSource}>();
const files = ref<File[]>([]);
const confirm = inject<TConfirm>("confirm",defaultConfirm);
const message = inject<IMessage>("message",defaultMessage)
const fileDataSource = useListDataSource(
  new ListDataSourceConfig({
    className:"bookingFile"
  })
)

fileDataSource.items.value = dataSource.model.value.files;
const upload = (file:File) => {
  files.value.push(file)
}
const finish = ({document, file}:any)=>{
  fileDataSource.items.value.push(document);
  files.value = files.value.filter(f => f.name !== file.name)
}

const remove = async (file:any) => {
  const confirmed = await confirm({
    message: 'Уверены что хотите удалить данный файл',
    header: 'Подтвердите свое действие',
  })
  if(!confirmed) return;
  await fileDataSource.remove(file.id);
  message.success("Файл успешно удалён")
}


</script>

