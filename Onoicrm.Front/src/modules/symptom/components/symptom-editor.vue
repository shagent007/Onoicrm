<template>
  <div
    v-for="symptom in symptomStore.items"
    :key="symptom.id"
    class="flex align-items-center mb-3"
  >
    <Checkbox
      :binary="true"
      :model-value="hasSymptom(symptom)"
      @input="updateSymptom(symptom, $event)"
      :inputId="'input-' + symptom.id"
    />
    <label :for="'input-' + symptom.id" class="ml-2">
      {{ symptom.caption }}
    </label>
  </div>
</template>

<script setup lang="ts">
import {IListDataSource, UpdateFieldModel} from "~/modules/data-sources";
import {useSymptomStore} from "~/modules/symptom";

const {dataSource, patientId} = defineProps<{dataSource:IListDataSource<any>, patientId:number}>();

const symptomStore= useSymptomStore();
const hasSymptom = (symptom: any) => {
  const defined: any = dataSource.items.value.find(
    (s: any) => s.symptomId == symptom.id,
  );
  if (!defined) return false;
  return defined.value;
};

const updateSymptom = async (symptom: any, event: any) => {
  const defined: any = dataSource.items.value.find(
    (s: any) => s.symptomId == symptom.id,
  );

  if (defined) return await dataSource.updateField(defined.id, [
    new UpdateFieldModel({
      fieldName: "value",
      fieldValue: event,
    }),
  ]);

  const data = await dataSource.add({
    patientId: patientId,
    symptomId: symptom.id,
    value: true,
  });
  dataSource.items.value.push(data);
};

</script>

