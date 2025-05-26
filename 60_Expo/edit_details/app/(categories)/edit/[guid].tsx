import React, { useEffect, useState } from 'react';
import { View, Text, TextInput, Switch, Button, ActivityIndicator } from 'react-native';
import { useForm, Controller, UseFormSetError, UseFormReset } from 'react-hook-form';
import { Picker } from '@react-native-picker/picker';
import { Router, useLocalSearchParams, useRouter } from 'expo-router';
import { getCategory, editCategory, EditCategoryFormData } from '@/utils/categories/apiClient';
import { ErrorResponse, isErrorResponse } from '@/utils/apiClient';
import { Category } from '@/types/Category';

// ----------------------------------------------
// Funktion außerhalb der Komponente
// ----------------------------------------------

async function editCategoryHandler(
  data: EditCategoryFormData, router: Router,
  setError: UseFormSetError<EditCategoryFormData>) {
  const result = await editCategory(data);

  if (!isErrorResponse(result)) {
    router.back();
    return;
  }

  if (result.validations) {
    Object.entries(result.validations).forEach(([field, message]) => {
      setError(field as keyof EditCategoryFormData, {
        type: 'server',
        message: message as string,
      });
    });
  }
  if (result.message) {
    alert(result.message);
  }
}
export async function loadCategory(
  guid: string,
  reset: UseFormReset<EditCategoryFormData>,
  router: Router
) {
  const result = await getCategory(guid);

  if (isErrorResponse(result)) {
    alert(result.message);
    router.back();
    return;
  }

  reset({
    name: result.name,
    description: result.description,
    isVisible: result.isVisible,
    priority: result.priority as "Low" | "Medium" | "High",
    guid: result.guid,
  });
}

// ----------------------------------------------
// Komponente
// ----------------------------------------------

export default function EditCategoryScreen() {
  const { guid } = useLocalSearchParams();
  const router = useRouter();
  const [loading, setLoading] = useState(true);

  const { control, handleSubmit, setError, reset, getValues, formState: { errors } } =
    useForm<EditCategoryFormData>({
      defaultValues: {
        name: '',
        description: '',
        isVisible: true,
        priority: 'Low',
      },
    });

  useEffect(() => {
    if (typeof guid === 'string') {
      loadCategory(guid, reset, router).finally(() => setLoading(false));
    }
  }, [guid]);

  if (loading) {
    return (
      <View style={{ padding: 20 }}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={{ padding: 20 }}>
      <Text>Name</Text>
      <Controller
        control={control}
        name="name"
        render={({ field: { onChange, value } }) => (
          <>
            <TextInput
              style={{ borderWidth: 1, marginBottom: 5, padding: 8 }}
              onChangeText={onChange}
              value={value}
            />
            {errors.name && (
              <Text style={{ color: 'red', marginBottom: 10 }}>
                {errors.name.message}
              </Text>
            )}
          </>
        )}
      />

      <Text>Beschreibung</Text>
      <Controller
        control={control}
        name="description"
        render={({ field: { onChange, value } }) => (
          <>
            <TextInput
              style={{ borderWidth: 1, marginBottom: 10, padding: 8 }}
              onChangeText={onChange}
              value={value}
            />
            {errors.description && (
              <Text style={{ color: 'red', marginBottom: 10 }}>
                {errors.description.message}
              </Text>
            )}
          </>
        )}
      />

      <Text>Priorität</Text>
      <Controller
        control={control}
        name="priority"
        render={({ field: { onChange, value } }) => (
          <View style={{ borderWidth: 1, marginBottom: 10 }}>
            <Picker
              selectedValue={value}
              onValueChange={(itemValue) => onChange(itemValue)}
            >
              <Picker.Item label="Low" value="Low" />
              <Picker.Item label="Medium" value="Medium" />
              <Picker.Item label="High" value="High" />
            </Picker>
          </View>
        )}
      />

      <View style={{ flexDirection: 'row', alignItems: 'center', marginBottom: 10 }}>
        <Text>Sichtbar</Text>
        <Controller
          control={control}
          name="isVisible"
          render={({ field: { onChange, value } }) => (
            <Switch value={value} onValueChange={onChange} />
          )}
        />
      </View>

      <Button title="Kategorie speichern" onPress={handleSubmit(() => editCategoryHandler(getValues(), router, setError))} />
    </View>
  );
}
