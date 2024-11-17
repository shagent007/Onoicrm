import {IListDataSource} from "~/modules/data-sources";
import {computed, isRef} from "vue";
import {INormalizedDataSource} from "~/modules/data-sources/interfaces/INormalizedDataSource";

export const unwrap = (dataSource: IListDataSource<any> | any): INormalizedDataSource => {

  const items = computed({
    get() {
      return isRef(dataSource.items) ? dataSource.items.value : dataSource.items
    },
    set(value) {
      if (isRef(dataSource.items)) {
        dataSource.items.value = value;
      } else {
        dataSource.items = value
      }
    }
  })
  const pageIndex = computed({
    get() {
      isRef(dataSource.pageIndex) ? dataSource.pageIndex.value : dataSource.pageIndex
    },
    set(value) {
      if (isRef(dataSource.pageIndex)) {
        dataSource.pageIndex.value = value;
      } else {
        dataSource.pageIndex = value
      }
    }
  })
  const pageSize = computed({
    get() {
      isRef(dataSource.pageSize) ? dataSource.pageSize.value : dataSource.pageSize
    },
    set(value) {
      if (isRef(dataSource.pageSize)) {
        dataSource.pageSize.value = value;
      } else {
        dataSource.pageSize = value
      }

    }
  })
  const orderFieldName = computed({
    get() {
      return isRef(dataSource.orderFieldName) ? dataSource.orderFieldName.value : dataSource.orderFieldName
    },
    set(value) {
      if (isRef(dataSource.orderFieldName)) {
        dataSource.orderFieldName.value = value;
      } else {
        dataSource.orderFieldName = value
      }

    }
  })
  const orderFieldDirection = computed({
    get() {
      return isRef(dataSource.orderFieldDirection) ? dataSource.orderFieldDirection.value : dataSource.orderFieldDirection
    },
    set(value) {
      if (isRef(dataSource.orderFieldDirection)) {
        dataSource.orderFieldDirection.value = value;
      } else {
        dataSource.orderFieldDirection = value
      }

    }
  })
  const loading = computed({
    get() {
      return isRef(dataSource.loading) ? dataSource.loading.value : dataSource.loading
    },
    set(value) {
      if (isRef(dataSource.loading)) {
        dataSource.loading.value = value;
      } else {
        dataSource.loading = value
      }

    }
  })
  const total = computed({
    get() {
      return isRef(dataSource.total) ? dataSource.total.value : dataSource.total
    },
    set(value) {
      if (isRef(dataSource.total)) {
        dataSource.total.value = value;
      } else {
        dataSource.total = value
      }

    }
  })
  const filter = computed({
    get() {
      return isRef(dataSource.filter) ? dataSource.filter.value : dataSource.filter
    },
    set(value) {
      if (isRef(dataSource.filter)) {
        dataSource.filter.value = value;
      } else {
        dataSource.filter = value
      }

    }
  })

  return {
    ...dataSource,
    items,
    pageIndex,
    pageSize,
    orderFieldName,
    orderFieldDirection,
    loading,
    total,
    filter
  }


}
