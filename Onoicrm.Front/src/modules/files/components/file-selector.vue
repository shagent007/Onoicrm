<template>
  <div
    @dragleave.prevent="dragEnter = false"
    @dragover.prevent="dragEnter = true"
    @drop.prevent.stop="onDrop"
    @change="change"
  >
    <label>
      <slot :dragEnter="dragEnter">
        Перетащите сюда файлы или выберите ...
      </slot>


      <input
        ref="fileRef"
        class="sr-only"
        hidden
        multiple
        type="file"
      />
    </label>
  </div>
</template>

<script setup lang="ts">
import {inject, ref} from "vue";
import {defaultMessage, IMessage} from "~/shared/models";
const {types,max} =  withDefaults(defineProps<{types?:string[], max?:number}>(), {
  types:()=>[],
  max:0
})
const fileRef = ref<HTMLInputElement>();
const dragEnter = ref<boolean>(false);
const message = inject<IMessage>("message",defaultMessage)
const emit = defineEmits(["select"]);

const change =({ target }: Event) => readFiles(target);

const onDrop =({ dataTransfer }: any) => {
  dragEnter.value = false;
  readFiles(dataTransfer);
}

const readFiles = async({ files }: any) => {
  const result = validateFiles(files);
  if (result !== "") {
    return message.error(result)
  }

  for (const file of files) {
    emit("select", file);
  }

  if(fileRef.value?.value != null) {
    fileRef.value.value = "";
  }
}

const validateFiles = (files: any) => {
  if (!files || files.length === 0) return "Загрузите файлы";
  const fileCount = files.length > max;

  if (max !== 0 && fileCount && fileRef.value) {
    fileRef.value.value = "";
    return `Вы не можете загрузить больше чем ${max} файлов`;
  }

  if (!(types?.length > 0)) {
    return "";
  }

  for (const file of files) {
    const isCorrect = validateFileType(file);
    if (!isCorrect) return "Загрузите правельный тип файла";
  }

  return "";
}

const validateFileType = ({ name }: File) => {
  let isCorrect = false;
  for (const fileType of types) {
    const fileExtension = name
      .substring(name.lastIndexOf(".") + 1)
      .toLowerCase();

    if (fileExtension === fileType.toLowerCase()) {
      isCorrect = true;
    }
  }

  return isCorrect;
}
</script>

