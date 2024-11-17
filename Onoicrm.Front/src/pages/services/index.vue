<template>
  <div v-if="loaded" class="grid relative mb-0 md:mb-3 lg:mb-0">
    <div class="col-12">
      <div class="grid">
        <div class="col-12 sm:col-3 flex-order-0 sm:flex-order-1 block lg:hidden">
          <div class="flex justify-content-start sm:justify-content-end">
            <Button
              v-if="appStore.buttons.length > 0"
              v-for="button in appStore.buttons"
              @click="button.action()"
              class="text-sm"
            >
              <div class="mr-2">
                <i class="pi pi-plus text-sm"></i>
              </div>
              Добавить
            </Button>
          </div>
        </div>
        <div class="col-12 sm:col-9">
          <div class="service-header-buttons">
            <div class="flex align-items-center">
              <Button
                @click="activeTab = 'general'"
                :severity="generalTabColor"
                label="Прейскурант"
                class="mr-3"
              />
              <Button
                @click="activeTab = 'preliminary'"
                :severity="preliminaryTabColor"
                label="Пред. прейскурант"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
    <div style="height: calc(100vh - 243px)" class="hidden lg:block col-3">
      <div
        style="background-color: white; border-radius: 10px"
        class="compact-tree no-filter h-full indent-tree"
      >
        <div
          style="
            height: calc(100vh - 300px);
            overflow-y: auto;
            overflow-x: hidden;
            border-radius: 10px;
          "
        >
          <Tree
            v-slot="{ node }"
            :value="serviceGroupDataSource.transformedRoot.value"
            v-model:selectionKeys="selectedKey"
            selectionMode="single"
            :meta-key-selection="false"
            @node-select="updateFilter($event)"
            @node-unselect="clearFilter()"
            v-model:expandedKeys="expandedKeys"
          >
            <context-menu v-slot="{ toggle }" :items="treeItems">
              <div class="flex align-items-center justify-content-between">
                <b
                  @contextmenu.prevent="handleClickBranch(toggle, $event, node)"
                  >{{ node.label }}</b
                >
                <div class="flex">
                  <Button
                    style="aspect-ratio: 1; width: 1.5rem"
                    class="p-0"
                    severity="secondary"
                    @click.stop="clearFilter"
                    outlined
                    size="small"
                    icon="pi pi-times"
                    v-if="isRoot(node)"
                  />

                  <div
                    v-else
                    :style="`
                        min-width:18px;
                        min-height:18px;
                        width:18px;
                        height:18px;
                        border-radius:4px;
                        background-color:${getItemColor(node.key)};
                      `"
                  />
                </div>
              </div>
            </context-menu>
          </Tree>
        </div>
      </div>
    </div>
    <div
      style="height: calc(100vh - 243px)"
      class="hidden md:block col-12 lg:col-9 mb-0 md:mb-3 lg:mb-0"
    >
      <div class="card block indent-table h-full" style="border-radius: 10px">
        <DataTable
          lazy
          @sort="dataSource.sort($event)"
          scrollable
          :reorderableColumns="hasFilter"
          @rowReorder="onRowReorder"
          scroll-height="calc(100vh - 300px)"
          :rows="dataSource.pageSize.value"
          :value="dataSource.activeItems.value"
        >
          <Column
            v-if="hasFilter"
            rowReorder
            headerStyle="width: 3rem"
            :reorderableColumn="false"
          />
          <Column field="id" header="#" />
          <Column field="code" header="Код" />
          <Column sortable field="caption" header="Наименование" />
          <Column v-if="activeTab === 'general'" field="price" header="Цена" />
          <Column
            v-if="activeTab !== 'general'"
            field="priceFrom"
            header="Цена от"
          />
          <Column
            v-if="activeTab !== 'general'"
            field="priceTo"
            header="Цена до"
          />
          <Column v-if="hasPermissions">
            <template #body="{ data }">
              <context-menu v-slot="{ toggle }" :items="items">
                <Button
                  icon="pi pi-ellipsis-v text-sm"
                  class="p-button-outlined p-button-sm btn-table"
                  @click="
                    currentItem = data;
                    toggle($event);
                  "
                />
              </context-menu>
            </template>
          </Column>
        </DataTable>
      </div>
    </div>
    <div
      v-touch:swipe.left="swipeLeft"
      v-touch:swipe.right="swipeRight"
      class="block md:hidden col-12 mb-3"
    >
      <div
        class="grid border-bottom-1 mb-3 items-center justify-content-between px-2 py-1 m-0"
      >
        <div class="col-2">
          <p class="mobile-card-description text-sm font-medium">#</p>
        </div>
        <div class="col-2 flex">
          <p class="mobile-card-description text-sm font-medium">code</p>
        </div>
        <div class="col-6 flex justify-content-evenly">
          <p
            class="mobile-card-description text-sm font-medium"
            v-if="activeTab === 'general'"
          >
            цена
          </p>
          <p
            class="mobile-card-description text-sm font-medium"
            v-if="activeTab !== 'general'"
          >
            цена от
          </p>
          <p
            class="mobile-card-description text-sm font-medium"
            v-if="activeTab !== 'general'"
          >
            цена до
          </p>
        </div>
        <div class="col-2">
          <small></small>
        </div>
      </div>
      <div
        v-for="item in dataSource.activeItems.value"
        class="card flex-column p-3 mb-3 indent-table"
      >
        <div class="w-full mb-2">
          <div class="grid">
            <div class="col-2">
              <p class="mobile-card-caption text-sm font-normal">
                {{ item.id }}
              </p>
            </div>
            <div class="col-2">
              <div>
                <p class="mobile-card-caption text-sm font-normal">
                  {{ item.code }}
                </p>
              </div>
            </div>
            <div class="col-6">
              <div class="flex items-center justify-content-evenly gap-4">
                <p
                  v-if="activeTab === 'general'"
                  class="mobile-card-caption text-sm font-normal"
                >
                  {{ item.price }}
                </p>
                <p
                  v-if="activeTab !== 'general'"
                  class="mobile-card-caption text-sm font-normal"
                >
                  {{ item.priceFrom }}
                </p>
                <p
                  v-if="activeTab !== 'general'"
                  class="mobile-card-caption text-sm font-normal"
                >
                  {{ item.priceTo }}
                </p>
              </div>
            </div>
            <div class="col-2 flex justify-content-end">
              <context-menu v-slot="{ toggle }" :items="items">
                <Button
                  @click="
                    currentItem = item;
                    toggle($event);
                  "
                  icon="pi pi-ellipsis-v text-xs md:text-sm"
                  class="p-button-outlined p-button-sm btn-table"
                />
              </context-menu>
            </div>
          </div>
        </div>
        <div class="flex align-items-center justify-content-between w-full">
          <h6 class="mobile-card-caption">
            {{ item.caption }}
          </h6>
        </div>
      </div>
    </div>
    <div class="col-12 relative">
      <Paginator
        class="datatable-paginator"
        :rows="dataSource.pageSize.value"
        :model-value="dataSource.pageIndex.value"
        :totalRecords="dataSource.total.value"
        @page="dataSource.paginate($event)"
        :template="{
          '640px': 'PrevPageLink CurrentPageReport NextPageLink',
          '960px':
            'FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink',
          '1300px':
            'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink',
          default:
            'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink',
        }"
      />
    </div>
  </div>
  <Sidebar
    v-model:visible="appStore.drawer"
    position="left"
    class="w-12 sm:w-8"
  >
    <div class="compact-tree no-filter h-full indent-tree">
      <div>
        <Tree
          v-slot="{ node }"
          :value="serviceGroupDataSource.transformedRoot.value"
          v-model:selectionKeys="selectedKey"
          selectionMode="single"
          :meta-key-selection="false"
          @node-select="updateFilter($event)"
          @node-unselect="clearFilter()"
          v-model:expandedKeys="expandedKeys"
        >
          <context-menu v-slot="{ toggle }" :items="treeItems">
            <div class="flex align-items-center justify-content-between">
              <b
                @contextmenu.prevent="handleClickBranch(toggle, $event, node)"
                >{{ node.label }}</b
              >
              <div class="flex">
                <Button
                  style="aspect-ratio: 1; width: 1.5rem"
                  class="p-0"
                  severity="secondary"
                  @click.stop="clearFilter"
                  outlined
                  size="small"
                  icon="pi pi-times"
                  v-if="isRoot(node)"
                />

                <div
                  v-else
                  :style="`
                        min-width:18px;
                        min-height:18px;
                        width:18px;
                        height:18px;
                        border-radius:4px;
                        background-color:${getItemColor(node.key)};
                  `"
                />
              </div>
            </div>
          </context-menu>
        </Tree>
      </div>
    </div>
  </Sidebar>
</template>

<script setup lang="ts">
import { useAppStore } from "~/modules/app/stores/useAppStore";
import { computed, inject, onMounted, reactive, Ref, ref, watch } from "vue";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import { defaultMessage, IMessage } from "~/shared";
import { TConfirm, defaultConfirm } from "~/shared";
import { useEditorStore } from "~/modules/editor/stores/useEditorDialog";
import { Service } from "~/modules/service/entities/Service";
import {
  Filter,
  ListDataSourceConfig,
  UpdateFieldModel,
  useListDataSource,
  useTreeDataSource,
} from "~/modules/data-sources";
import { TreeDataSourceConfig } from "~/modules/data-sources/hooks/useTreeDataSource";
import {
  collectTreeBranchParents,
  findElementInTree,
  removeElementInTree,
  updateElementInTree,
} from "~/shared/lib/helpers";
import { useServiceGroupLinkDataSource } from "~/modules/service-group-link/data-sources/useServiceGroupLinkDataSource";
import { hasStringValue } from "~/shared/lib/hasStringValue";
import ContextMenu from "~/shared/components/context-menu.vue";
import Tree, { TreeNode } from "primevue/tree";
import DataTable from "primevue/datatable";
import { ButtonModel } from "~/modules/app/models/ButtonModel";
import { Group } from "~/shared/entities/Group";
import { useUserStore } from "~/modules/user/stores/useUserStore";
import { getServiceGroupConfig } from "~/modules/service-group/forms/getCreateItemForm";
import {
  getServiceCreateItemForm,
  getServiceUpdateItemForm,
} from "~/modules/service/forms/getServiceForm";
import { ServicePriceType } from "~/modules/service/models/ServicePriceType";
import moment from "moment/moment";
import { genderCaption } from "~/modules/patient/services/getGenderCaption";

const message = inject<IMessage>("message", defaultMessage);
const confirm = inject<TConfirm>("confirm", defaultConfirm);

const userStore = useUserStore();
const appStore = useAppStore();
const clinicStore = useClinicStore();
const editorStore = useEditorStore();

const isRoot = (node: any) => {
  const branch = findElementInTree(node.key, serviceGroupDataSource.root.value);
  return (
    !branch.parentId && dataSource.filter.value.some((s) => s.name == "groupId")
  );
};
const dataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "service",
    filter: [new Filter("clinicId", clinicStore.clinic?.id)],
  })
);

const serviceGroupLinkDataSource = useServiceGroupLinkDataSource();

const serviceGroupDataSource = useTreeDataSource(
  new TreeDataSourceConfig({
    className: "ServiceGroup",
  })
);

const loaded = ref<boolean>(false);
const activeTab = ref<ServicePriceType>("general");
const timeOut = ref();
const currentNode: Ref<TreeNode | any> = ref(null);
const currentItem: Ref = ref();
const selectedKey: Ref = ref(0);
const expandedKeys = ref<any>({});

const generalTabColor = computed(() =>
  activeTab.value == "general" ? "primary" : "secondary"
);
const preliminaryTabColor = computed(() =>
  activeTab.value == "preliminary" ? "primary" : "secondary"
);

const hasPermissions = computed(() =>
  userStore.hasOptionalRoles([
    "System Administrator",
    "Administrator",
    "Director",
  ])
);

const handleClickBranch = (toggle: any, event: any, node: any) => {
  if (hasPermissions.value) {
    toggle(event);
    selectNode(node);
  }
};
const selectNode = (node: TreeNode) => {
  currentNode.value = node;
  selectedKey.value = { [currentNode.value.key]: true };
};
const getItemColor = (id: number) => {
  const color = findElementInTree(
    id,
    serviceGroupDataSource.root.value
  )?.textColor;
  if (!hasStringValue(color)) return "";
  return color;
};

const swipeLeft = async () => {
  if (
    dataSource.pageIndex.value ===
    Math.ceil(dataSource.total.value / dataSource.pageSize.value)
  )
    return;
  dataSource.pageIndex.value++;
  await dataSource.get();
};

const swipeRight = async () => {
  if (dataSource.pageIndex.value == 1) return;
  dataSource.pageIndex.value--;
  await dataSource.get();
};

const hasFilter = computed(() =>
  dataSource.filter.value.some((f) => f.name == "groupId")
);

const treeItems: Ref<any[]> = ref([
  {
    label: "Добавить",
    command: async () => {
      const model = reactive<Group>(new Group());
      model.parentId = +currentNode.value.key;
      const config = getServiceGroupConfig(model, "Добавить категорию");
      const result = await editorStore.create(config);
      if (!result) return;
      await serviceGroupDataSource.add(result);
      await serviceGroupDataSource.getRoot(
        clinicStore.clinic?.id as number
      );
      message.success("Успешно добавлено");
    },
  },
  {
    label: "Редактировать",
    command: async () => {
      const model = findElementInTree(
        +currentNode.value.key,
        serviceGroupDataSource.root.value
      );
      const config = getServiceGroupConfig(model, "Редактировать группу");
      const result = await editorStore.update(config);
      if (!result) return;
      await serviceGroupDataSource.update(result);
      updateElementInTree(
        result,
        serviceGroupDataSource.root.value,
        (branch: any) => Object.assign(branch, result)
      );
      message.success("Успешно обновлено");
    },
  },
  {
    label: "Удалить",
    command: async () => {
      const confirmed = await confirm({
        header: "Подвердите своё действие",
        message: "Вы действительно хотите удалить данну группу?",
      });
      if (!confirmed) return;
      await serviceGroupDataSource.remove(+currentNode.value.key);
      removeElementInTree(
        +currentNode.value.key,
        serviceGroupDataSource.root.value
      );
      message.success("Успешно удалено");
    },
  },
]);

const onRowReorder = async (event: any) => {
  if (!hasFilter.value) return;
  dataSource.items.value = event.value;
  await dataSource.updatePriorities();
};

const updateFilter = async (data: any) => {
  await dataSource.updateFilter([new Filter("groupId", data.key)]);
};

const clearFilter = async () => {
  selectedKey.value = null;
  await dataSource.removeFilter(["groupId"]);
};

const items = ref([
  {
    label: "Редактировать",
    command: async () => {
      const config = getServiceUpdateItemForm(
        currentItem.value,
        dataSource,
        serviceGroupDataSource,
        serviceGroupLinkDataSource,
        activeTab.value
      );
      await editorStore.update(config);
    },
  },
  {
    label: "Удалить",
    command: async () => {
      const _confirm = await confirm({
        header: "Подтвердите свое действие",
        message: "Вы действительно хотите удалить данный обьект?",
      });
      if (!_confirm) return;
      await dataSource.updateField(currentItem.value.id, [
        new UpdateFieldModel({
          fieldName: "stateId",
          fieldValue: 3,
        }),
      ]);
      message.success("Успешно удалено");
    },
  },
]);

watch(
  () => appStore.searchText,
  async (newValue: string) => {
    clearTimeout(timeOut.value);
    timeOut.value = setTimeout(async () => {
      await dataSource.updateFilter([new Filter("searchText", newValue)]);
    }, 1300);
  }
);

if (hasPermissions.value) {
  appStore.buttons = [
    new ButtonModel({
      attrs: {
        icon: "pi pi-plus",
        class: " px-4 border-round-xl",
      },
      action: async () => {
        const key = +Object.keys(selectedKey.value)?.[0];

        if (!key) {
          message.error("Выберите категорию");
          return;
        }

        const group = findElementInTree(key, serviceGroupDataSource.root.value);

        const links = collectTreeBranchParents(
          group,
          serviceGroupDataSource.root.value
        )
          .map((g: any) => ({ serviceGroupId: g.id }))
          .reverse();

        const config = getServiceCreateItemForm(
          new Service({
            clinicId: clinicStore.clinic?.id,
            stateId: 2,
            links,
          }),
          serviceGroupDataSource,
          serviceGroupLinkDataSource,
          activeTab.value
        );

        const data = await editorStore.create(config);
        const result = await dataSource.add(data);
        dataSource.items.value.push(result);
        message.success("Успешно добавлено");
      },
    }),
  ];
}

onMounted(async () => {
  try {
    await Promise.all([
      serviceGroupDataSource.getRoot(clinicStore.clinic?.id as number),
      dataSource.get(),
    ]);
  } catch (e) {
    throw e;
  }
  loaded.value = true;
});
</script>
