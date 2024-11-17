import {collectTreeBranchParents, findElementInTree, remove, treeToFlat} from "~/shared/lib/helpers";
import {cloneDeep} from "lodash";
import {IListDataSource, ITreeDataSource} from "~/modules/data-sources";
import {IServiceGroupLinkDataSource} from "~/modules/service-group-link/interfaces/IServiceGroupLinkDataSource";

export const getServiceLinkActions = (dataSource:IServiceGroupLinkDataSource, serviceGroupDataSource:ITreeDataSource) => ({
    getServiceLinkConfig: () => ({dataSource: serviceGroupDataSource}),
    getServiceLinkItemConfig: () => {
    const groups = treeToFlat(serviceGroupDataSource.root.value[0]);
    return {
      getCaption: (item: any) => {
        return groups
          .find((g: any) =>g.id == item.serviceGroupId)
          ?.caption;
      },
    }
  },
    addServiceLink: async (branch: any, model: any) => {
      const definedEl =  findElementInTree(branch, serviceGroupDataSource.root.value);
      const group = cloneDeep(definedEl);
      if (group?.children?.length) group.children = [];
      const groups = collectTreeBranchParents(group, serviceGroupDataSource.root.value,)
        .filter((g: any) => !model.links.some(
            (l: any) => l?.serviceGroupId == g.id,
          ),
        );
      const upg = groups.map((g: any) => ({
        serviceGroupId: g.id,
        serviceId: model.id,
      }));
      const items: any = await dataSource.cascadeAdd(upg);
      model.links = [...model.links, ...items];
    },
    deleteServiceLink: async (branch: any, model: any) => {
      const id = branch.classId == 24 ? branch.id : branch?.serviceGroupId;
      const definedEl = findElementInTree(id, serviceGroupDataSource.root.value);
      const group = cloneDeep(definedEl);
      const groups = treeToFlat(group);
      const links = model.links.filter((l: any) => groups.some((g) => g.id == l.serviceGroupId));
      await dataSource.cascadeRemove(links);
      for (const link of links) {
        remove(model.links, (i: any) => i.id == link.id);
      }
    },
    getServiceLinkFilterGroups: (item: any) =>{
      return (i: any) => i.serviceGroupId == item;
    }
})
