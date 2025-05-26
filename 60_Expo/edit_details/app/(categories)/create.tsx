import React from 'react';
import { View, Text, TextInput, Switch, Button } from 'react-native';
// npm install react-hook-form
import { useForm, Controller, UseFormSetError } from 'react-hook-form';
import { addCategory, NewCategoryFormData } from '@/utils/categories/apiClient';
import { isErrorResponse } from '@/utils/apiClient';
import { Router, useRouter } from 'expo-router';
// npm install @react-native-picker/picker
import { Picker } from '@react-native-picker/picker';

async function addCategoryHandler(
    data: NewCategoryFormData, router: Router, setError: UseFormSetError<NewCategoryFormData>) {
  const result = await addCategory(data);
  if (isErrorResponse(result)) {
    if (result.validations) {
      Object.entries(result.validations).forEach(([field, message]) => {
        setError(field as keyof NewCategoryFormData, {
          type: 'server',
          message: message as string,
        });
      });
    } else {
      alert(result.message);
    }
    return;
  }

  router.back();
}

export default function CreateCategoryScreen() {
  const router = useRouter();
  const { control, handleSubmit, setError, getValues, formState: { errors } } = useForm<NewCategoryFormData>({
    defaultValues: {
      name: '',
      description: '',
      isVisible: true,
      priority: 'Low',
    },
  });

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

      <Button title="Kategorie erstellen" onPress={handleSubmit(() => addCategoryHandler(getValues(), router, setError))} />
    </View>
  );
}
