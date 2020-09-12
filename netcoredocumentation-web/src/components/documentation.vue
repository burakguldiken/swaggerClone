<template>
  <div>
    <v-card color="grey lighten-4" flat height="300px" tile>
      <v-toolbar dark>
        <v-col cols="2">
          <v-spacer></v-spacer>
        </v-col>
        <v-col cols="1">
          <v-img class="white--text image" src="@/assets/swagger.svg"></v-img>
        </v-col>
        <v-col cols="4">
          <v-spacer></v-spacer>
        </v-col>
        <v-col cols="1">
          <span>Select a defination</span>
        </v-col>
        <v-col class="mt-6" cols="3">
          <v-combobox v-model="selectedVersion" :items="versions" multiple outlined dense></v-combobox>
          <v-spacer></v-spacer>
        </v-col>
      </v-toolbar>
      <v-row class="mt-8">
        <v-col cols="6" align="center" justify="center">
          <span style="font-weight:bold; font-size:30px">
            SWAGGER CLONE APP
            <v-chip class="mb-6">V1</v-chip>
            <v-chip class="mb-6 ml-2" color="green" text-color="white">OAS3</v-chip>
          </span>
          <br />
          <span
            style="color:#4990e2; font-size:12px; margin-right:100px"
          >/swagger/CBSTHCRM/swagger.json</span>
          <br />
          <span style="margin-right:100px">Swagger Clone App Web Api</span>
        </v-col>
      </v-row>
    </v-card>
    <v-container>
      <v-row class="mb-6 mt-6">
        <v-col cols="12">
          <span style="font-weight:bold; font-size:30px">{{key}}</span>
        </v-col>
        <v-col cols="12">
          <v-divider></v-divider>
        </v-col>
      </v-row>
      <v-expansion-panels>
        <v-expansion-panel
          v-for="(item,method) in documentList"
          :key="method"
          style="border-radius:10px; margin-top:10px;"
        >
          <div :style="item.methodBackgroundColor">
            <v-expansion-panel-header disable-icon-rotate>
              <v-row no-gutters>
                <v-col cols="1">
                  <v-btn small :style="item.color">
                    <span class="white--text">{{item.ActionName}}</span>
                  </v-btn>
                </v-col>
                <v-col cols="3">
                  <span
                    class="font-weight-black"
                  >/api/{{item.Controller}}/{{item.ActionDescription}}</span>
                </v-col>
                <v-col cols="8">
                  <span>{{item.Summary}}</span>
                </v-col>
              </v-row>
              <template v-slot:actions>
                <v-icon class="ml-6" color="blue" small>fas fa-lock</v-icon>
              </template>
            </v-expansion-panel-header>
          </div>
          <v-expansion-panel-content>
            <v-row>
              <v-col cols="12">
                <v-btn @click="execute(item,values)" block style="background:#41444e; color:white">
                  <span style="text:center">Execute</span>
                </v-btn>
              </v-col>
            </v-row>
            <v-row v-if="item.ParametersName !== null">
              <v-col
                v-for="(parameter,parameters) in item.ParametersName"
                :key="parameters"
                cols="1"
                class="mt-2"
              >
                <span>{{parameter}}</span>
              </v-col>
              <v-col cols="2">
                <v-text-field v-model="values" :label="item.ParametersType" outlined dense></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <span class="font-weight-black ml-2 mt-8">Response</span>
              <v-col cols="12">
                <v-card class="ma-2" style="border-radius:4px; background:#41444e; color:white">
                  <v-row v-for="(result,results) in executeResult" :key="results">
                    <v-col cols="8">
                      <span class="font-weight-black ml-3">{{results}} :</span>
                      <span>{{result}}</span>
                    </v-col>
                  </v-row>
                </v-card>
              </v-col>
            </v-row>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-container>
  </div>
</template>

<script>
import { GET_DOCUMENTATION, GET_EXECUTE } from "@/store/actions.type";
export default {
  data() {
    return {
      documentList: [],
      key: "",
      selectedVersion: ["Swagger Clone App"],
      versions: ["Swagger Clone App"],
      executeResult: {},
      values: null,
    };
  },

  methods: {
    execute(item, values) {

      if (values !== null) {
        if (item.ParametersType[0] === "String") 
        {
          values = values.toString();
        }
        else if(item.ParametersType[0] === "Int32") {
          values = parseInt(values);
        }
      }

      const request = {
        item: item,
        values: values,
      };
      
      this.$store
        .dispatch(GET_EXECUTE, request)
        .then(() => {
          this.executeResult = this.$store.getters.getExecuteInfo;
        })
        .catch((err) => {
          this.$swal("Error", err.errorMessage, "error");
        });
    },
  },

  created() {
    this.$store
      .dispatch(GET_DOCUMENTATION)
      .then(() => {
        this.key = this.$store.getters.getDocumentKeyInfo;
        this.documentList = this.$store.getters.getDocumentValueInfo;
        this.documentList.forEach((item) => {
          if (item.ActionName === "HttpPost") {
            item.color = "background:#49cc90";
            item.methodBackgroundColor =
              "border-color: #49cc90;background: rgba(73,204,144,.1);";
            item.ActionName = "Post";
          } else if (item.ActionName === "HttpGet") {
            item.color = "background:#61affe";
            item.methodBackgroundColor =
              "border-color: #61affe;background: rgba(97,175,254,.1);";
            item.ActionName = "Get";
          } else if (item.ActionName === "HttpPut") {
            item.color = "background:#fca130;";
            item.methodBackgroundColor =
              "border-color: #fca130;background: rgba(252,161,48,.1);";
            item.ActionName = "Put";
          } else if (item.ActionName === "HttpDelete") {
            item.color = "background:#f93e3e";
            item.methodBackgroundColor =
              "border-color: #f93e3e;background: rgba(249,62,62,.1);";
            item.ActionName = "Delete";
          }
        });
      })
      .catch(() => {
        console.log("Error");
      });
  },
};
</script>