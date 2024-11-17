<template>
  <div
    class="card--darkblue p-3 h-full flex flex-column justify-content-between pb-4"
  >
    <div>
      <div class="flex align-items-center justify-content-between mb-4">
        <h3>Счёт</h3>


        <div @click="add" class="flex align-items-center cursor-pointer">
          <i class="pi pi-plus pr-2"/> Добавить
        </div>
      </div>
      <div class="h-17rem overflow-auto mb-3 pr-3">
        <div
          v-for="item in priceDataSource.items.value as any[]"
          :key="item.id"
          class="flex align-items-end justify-content-between mb-3 border-none border-bottom-1 pb-2 mb-4"
        >
          <div class="price-input">
            <inline-input
              v-slot="{edit}"
              v-model="item.caption"
              @update="updateField(item,'caption')"
            >
              <p @click="edit()">{{item.caption }}</p>
            </inline-input>
          </div>

          <div class="flex align-items-center">
            <div class="price-input price-input--number">
              <inline-input
                  v-slot="{edit}"
                  v-model="item.value"
                  @update="updateField(item,'value')"
              >
                <h4 @click="edit()">{{ item.value }}</h4>

              </inline-input>
            </div>
            <i
                class="pi pi-trash cursor-pointer block ml-4"
                style="color: red; font-size: 16px; margin-top:1px;"
                @click="remove(item)"
            />
          </div>

        </div>
      </div>
    </div>
    <div class="flex align-items-center justify-content-between mt-3">
      <h3>Итого:</h3>
      <h3>{{ total }} руб</h3>
    </div>
  </div>
</template>


<script setup lang="ts">
import {
  IObjectDataSource,
  ListDataSourceConfig,
  UpdateFieldModel,
  useListDataSource
} from "~/modules/data-sources";
import {computed, inject, ref} from "vue";
import {defaultMessage, IMessage} from "~/shared";
import InlineInput from "~/shared/ui/inline-input.vue";

const message = inject<IMessage>("message",defaultMessage);
const {dataSource} = defineProps<{dataSource:IObjectDataSource}>();
const priceDataSource = useListDataSource(
  new ListDataSourceConfig({
    className:"price",
  })
)

priceDataSource.items.value = dataSource.model.value?.prices;
const total = computed(()=>priceDataSource.items.value.reduce((a, b:any) => a + Number(b.value), 0))
const add = async () => {
  const data = await priceDataSource.add({
    bookingId:dataSource.model.value.id,
    caption:"Новая цена " + priceDataSource.items.value.length
  })
  priceDataSource.items.value.push(data);
}
const d = ref('')

const updateField = async (item:any,name:string) => {
  await priceDataSource.updateField(item.id,[
    new UpdateFieldModel({
      fieldName:name,
      fieldValue: item[name]
    })
  ]);
  message.success("Данные успешно обновлены")
}

const remove = async (item:any) => {
  await priceDataSource.remove(item.id);
  message.success("Элемент успешно удалён")
}
</script>
